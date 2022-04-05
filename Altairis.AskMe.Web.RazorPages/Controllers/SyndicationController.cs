using System.Text;
using System.Text.Encodings.Web;
using System.Xml;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;

namespace Altairis.AskMe.Web.RazorPages.Controllers {
    public class SyndicationController : Controller {
        private const int TITLE_MAX_LENGTH = 50;
        private const int DESCRIPTION_MAX_LENGTH = 200;

        private readonly AskDbContext _dc;
        private readonly HtmlEncoder _encoder;

        public SyndicationController(AskDbContext dc, HtmlEncoder encoder) {
            this._dc = dc;
            this._encoder = encoder;
        }

        [Route("/feed.rss", Name = "RssFeed")]
        public async Task<IActionResult> RssFeed() {
            var homepageUrl = this.Url.Page("/Index", pageHandler: null, values: null, protocol: this.Request.Scheme);
            var items = await this.GetSyndicationItemsAsync(this.Request.Scheme, 15);

            using (var sw = new StringWriter()) {
                var settings = new XmlWriterSettings {
                    Async = true,
                    Indent = true,
                    Encoding = Encoding.UTF8,
                    OmitXmlDeclaration = true
                };
                using (var xmlWriter = XmlWriter.Create(sw, settings)) {
                    var writer = new RssFeedWriter(xmlWriter);
                    await writer.WriteTitle("ASKme");
                    await writer.WriteDescription("Zeptej se mě na co chceš, já na co chci odpovím");
                    await writer.Write(new SyndicationLink(new Uri(homepageUrl)));
                    await writer.WritePubDate(DateTimeOffset.UtcNow);

                    foreach (var item in items) {
                        await writer.Write(item);
                    }

                    xmlWriter.Flush();
                }
                return this.File(Encoding.UTF8.GetBytes(sw.ToString()), "application/rss+xml;charset=utf-8");
            }
        }

        private async Task<IEnumerable<SyndicationItem>> GetSyndicationItemsAsync(string protocol, int maxItems) {
            var questions = await this._dc.Questions.Include(x => x.Category)
                .Where(x => x.DateAnswered.HasValue)
                .OrderByDescending(x => x.DateAnswered)
                .Take(maxItems)
                .ToListAsync();

            return questions.Select(q => {
                var item = new SyndicationItem {
                    Title = TruncateString(q.QuestionText, TITLE_MAX_LENGTH),
                    Description = this._encoder.Encode(TruncateString(q.QuestionText, DESCRIPTION_MAX_LENGTH)),
                    Id = this.Url.Page("/Question", pageHandler: null, values: new { questionId = q.Id }, protocol: protocol),
                    Published = q.DateAnswered.Value
                };
                item.AddCategory(new SyndicationCategory(q.Category.Name));
                return item;
            });
        }

        private static string TruncateString(string s, int maxLength) {
            if (s == null) throw new ArgumentNullException(nameof(s));

            if (s.Length >= maxLength) s = s.Substring(0, maxLength) + "...";
            return s;
        }

    }
}
