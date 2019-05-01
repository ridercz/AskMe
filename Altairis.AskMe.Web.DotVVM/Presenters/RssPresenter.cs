using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Xml;
using Altairis.AskMe.Data;
using DotVVM.Framework.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System.IO;

namespace Altairis.AskMe.Web.DotVVM.Presenters {
    public class RssPresenter : IDotvvmPresenter {
        private const int TITLE_MAX_LENGTH = 50;
        private const int DESCRIPTION_MAX_LENGTH = 200;

        private readonly AskDbContext _dc;
        private readonly HtmlEncoder _encoder;
        private readonly IDotvvmRequestContext context;

        public RssPresenter(AskDbContext dc, HtmlEncoder encoder, IDotvvmRequestContext context) {
            this._dc = dc;
            this._encoder = encoder;
            this.context = context;
        }

        private async Task<IEnumerable<SyndicationItem>> GetSyndicationItemsAsync(int maxItems) {
            var questions = await this._dc.Questions.Include(x => x.Category)
                .Where(x => x.DateAnswered.HasValue)
                .OrderByDescending(x => x.DateAnswered)
                .Take(maxItems)
                .ToListAsync();

            return questions.Select(q => {
                var item = new SyndicationItem {
                    Title = TruncateString(q.QuestionText, TITLE_MAX_LENGTH),
                    Description = this._encoder.Encode(TruncateString(q.QuestionText, DESCRIPTION_MAX_LENGTH)),
                    Id = this.GetAbsoluteUri(this.context.Configuration.RouteTable["QuestionDetail"].BuildUrl(new { questionId = q.Id })).ToString(),
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

        public Uri GetAbsoluteUri(string path = "") {
            var rq = this.context.GetAspNetCoreContext().Request;
            var builder = new UriBuilder() { Scheme = rq.Scheme, Host = rq.Host.Host };
            if (rq.Host.Port.HasValue) builder.Port = rq.Host.Port.Value;
            if (!string.IsNullOrEmpty(path)) builder.Path = this.context.TranslateVirtualPath(path);
            return builder.Uri;
        }

        public async Task ProcessRequest(IDotvvmRequestContext context) {
            var items = await this.GetSyndicationItemsAsync(15);

            using (var sw = new StreamWriter(this.context.HttpContext.Response.Body, Encoding.UTF8, 1024, true)) {
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
                    await writer.Write(new SyndicationLink(this.GetAbsoluteUri()));
                    await writer.WritePubDate(DateTimeOffset.UtcNow);

                    foreach (var item in items) {
                        await writer.Write(item);
                    }

                    await writer.Flush();
                    xmlWriter.Flush();

                }
            }
        }

    }
}
