function setElementAttributeById(elementId, attributeName, attributeValue) {
	var element = window.document.getElementById(elementId);
	element.setAttribute(attributeName, attributeValue);
}
function setElementAttribute(element, attributeName, attributeValue) {
	element.setAttribute(attributeName, attributeValue);
}

function setPageTitle(title) {
	document.title = title;
}