
function InitTitle()
{
	if ( document == null ) return;
	if ( document.body == null ) return;

	InsertHeader();
		
	InsertFooter();
}

function InsertHeader()
{
    //setting
    var documentationTitle = "Ferda DataMiner developer's documentation";

    var html = "<div id=\"nsbanner\"><div id=\"bannerrow1\">";
    html += "<table class=\"bannerparthead\" cellspacing=\"0\"><tr id=\"hdr\"><td class=\"runninghead\">";
    html += documentationTitle;
    html += "</td><td class=\"product\"></td></tr></table>";
    html += "</div><div id=\"TitleRow\"><h1 class=\"dtH1\">";
    html += document.title;
    html += "</h1></div></div>";
    
	document.body.insertAdjacentHTML( "afterBegin", html );
}

function InsertFooter()
{
	var lastChild = document.body.lastChild;
	if ( lastChild == null ) return;

    //setting
    var documentationTitle = "Ferda DataMiner developer's documentation";
    var documentationCopyrightText = "Copyright (C) 2005 Michal Kovac, Tomas Kuchar, Alexander Kuzmin, Martin Ralbovsky";
    var documentationFeedbackEmailAddress = "ferda-users@lists.sourceforge.net";
    
	var subject = "?subject=" + documentationTitle + " " + document.title + "'"
	subject = subject.replace("(\s+)","%20");

    var html = "<hr /><div id=\"footer\"><p><a href=\"mailto:"
    html += documentationFeedbackEmailAddress;
    html += "%20?subject=";
    html += subject;
    html += "\">Send comments on this topic.</a>";
    html += "</p><p><a>" + documentationCopyrightText + "</a></p><p></p></div>";

	lastChild.insertAdjacentHTML( "beforeEnd", html );
}