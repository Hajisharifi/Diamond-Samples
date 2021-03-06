--DBMS: Oracle
--APP:  Oracle SQL Developer

--DROP TABLE BBCODES;
--COMMIT;

--TRUNCATE TABLE BBCODES;
--COMMIT;

CREATE TABLE BBCODES
(
	TAGNAME        VARCHAR2(50) NOT NULL,
	RAZORTEMPLATE  NVARCHAR2(2000)      ,
	VALIDATIONCODE NVARCHAR2(2000)      ,
	CONSTRAINT BBCODES_PK PRIMARY KEY (TAGNAME) ENABLE 
);
COMMIT;

SET DEFINE OFF;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('b', N'<b class="bbcode-b">@RenderBody()</b>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('i', N'<i class="bbcode-i">@RenderBody()</i>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('u', N'<u>@RenderBody()</u>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('s', N'<s>@RenderBody()</s>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('center', N'<center class="bbcode-center">@RenderBody()</center>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('left', N'<div class="bbcode-left" align="left">@RenderBody()</div>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('right', N'<div class="bbcode-right" align="right">@RenderBody()</div>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('ltr', N'<div class="bbcode-ltr" dir="ltr">@RenderBody()</div>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('rtl', N'<div class="bbcode-rtl" dir="rtl">@RenderBody()</div>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('size', N'<div class="bbcode-size" style="font-size: @(5*Node.Attrib().Clamp(1,9,2))px">@RenderBody()</div>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;
      if (!Node.Attrib().IsInRange(1,9)) return TagValidation.Ignore;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('code', N'<pre class="bbcode-code" dir="ltr">@RenderBody()</pre>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('color', N'<div class="bbcode-color" style="color: @Node.Attrib()">@RenderBody()</div>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;
      if (Node.Attrib().IsEmpty()) return TagValidation.Ignore;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('email', N'<a href="mailto:@Node.Attrib().IIf(Node.Inner)" target="_blank">@RenderBody().IIf(Node.Attrib())</a>', N'if (Node.Inner.IsEmpty() && Node.Attrib().IsEmpty()) return TagValidation.Ignore;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('font', N'<div class="bbcode-font" style="font-family: @Node.Attrib()">@RenderBody()</div>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;
      if (Node.Attrib().IsEmpty()) return TagValidation.Ignore;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('img', N'@{
      int w = Node.Attributes["width"].Clamp(10, 500, 0);
      int h = Node.Attributes["height"].Clamp(10, 500, 0);
      }
      <div class="bbcode-img"><img
      @(w > 0 ? string.Format("width=''{0}px''", w) : "")
      @(h > 0 ? string.Format("height=''{0}px''", h) : "")
      src="@Node.Attrib().IIf(Node.Inner)"
      alt=""
      border="0"
      onload="$.BBCode.ImgLoad(this);" /></div>', N'if (Node.Inner.IsEmpty() && Node.Attrib().IsEmpty()) return TagValidation.Remove;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('quote', N'<div class="bbcode-quote">
      <div>
      <img title="quote" alt="quote" src="/Content/bbcode-quote.png">
      @if(!Node.Attrib().IsEmpty()){ <text>Originally posted by <strong>@Node.Attrib()</strong></text> }
      </div>
      @RenderBody()
      </div>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('table', N'<table>@RenderBody(true)</table>', N'if (Node.Childs.IsEmpty()) return TagValidation.Remove;
      if (!Node.AllChilds("tr")) return TagValidation.Ignore;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('tr', N'<tr>@RenderBody(true)</tr>', N'if ((!Node.HasParent("table")) || (!Node.AllChilds("td","th"))) return TagValidation.Ignore;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('th', N'<th width="@(((object)Node.Attribs.Width).Clamp(10, 300, "auto"))">@RenderBody()</th>', N'if (!Node.HasParent("tr")) return TagValidation.Ignore;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('td', N'<td width="@(((object)Node.Attribs.Width).Clamp(10, 300, "auto"))">@RenderBody()</td>', N'if (!Node.HasParent("tr")) return TagValidation.Ignore;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('url', N'<a class="bbcode-url" target="_blank" href="@Node.Attrib().IIf(Node.Inner)">@RenderBody().IIf(Node.Attrib())</a>', N'if (Node.Inner.IsEmpty() && Node.Attrib().IsEmpty()) return TagValidation.Ignore;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('ul', N'<ul>@RenderBody(true)</ul>', N'if (Node.Childs.IsEmpty()) return TagValidation.Remove;
      if (!Node.AllChilds("li","*")) return TagValidation.Ignore;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('ol', N'<ol>@RenderBody(true)</ol>', N'if (Node.Childs.IsEmpty()) return TagValidation.Remove;
      if (!Node.AllChilds("li","*")) return TagValidation.Ignore;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('list', N'<ul>@RenderBody(true)</ul>', N'if (Node.Childs.IsEmpty()) return TagValidation.Remove;
      if (!Node.AllChilds("li","*")) return TagValidation.Ignore;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('li', N'<li>@RenderBody()</li>', N'if (!Node.HasParent("ul","ol","list")) return TagValidation.Ignore;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('*', N'<li>@RenderBody()</li>', N'if (!Node.HasParent("ul","ol","list")) return TagValidation.Ignore;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('sub', N'<sub>@RenderBody()</sub>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('sup', N'<sup>@RenderBody()</sup>', N'if (Node.Inner.IsEmpty()) return TagValidation.Remove;');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('hr', N'<hr/>@RenderBody()', N'');
COMMIT;

INSERT INTO BBCODES (TAGNAME, RAZORTEMPLATE, VALIDATIONCODE)
VALUES ('youtube', N'<div class="bbcode-youtube">
      <iframe width="500" height="400" src="https://www.youtube.com/embed/@Node.Inner.IIf(Node.Attrib())" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>
      </div>', N'if (Node.Inner.IsEmpty() && Node.Attrib().IsEmpty()) return TagValidation.Remove;');
COMMIT;
