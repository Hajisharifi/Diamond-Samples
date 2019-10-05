/// <reference path="jquery-1.12.4.js" />

$.BBCode = $.BBCode || {};
$.BBCode.IMG_MaxWidth = 400;
$.BBCode.IMG_MsgSmallToLarge = "Click here to view the large image.";
$.BBCode.IMG_MsgLargeToSmall = "Click here to view the small image.";
//________________________________________________________________________

$.BBCode.ImgLoad = function (img) {
    if (img.originalWidth) img.orgWidth = img.originalWidth;
    else img.orgWidth = img.width;

    if (img.orgWidth <= $.BBCode.IMG_MaxWidth) return;

    var hyperlink = $("<div onclick='$.BBCode.ImgToggle(this);' class='bbcode-imageresizer-warning'></div>");

    hyperlink.insertBefore($(img));

    hyperlink[0].img = img;

    img.isSmall = false;

    $.BBCode.ImgToggle(hyperlink[0]);
};
//________________________________________________________________________

$.BBCode.ImgToggle = function (hyperlink) {
    hyperlink = $(hyperlink);
    var img = $(hyperlink[0].img);
    var curWidth = 0;

    img[0].isSmall = !img[0].isSmall;
    if (img[0].isSmall) {
        curWidth = $.BBCode.IMG_MaxWidth;
        hyperlink.text($.BBCode.IMG_MsgSmallToLarge);
    }
    else {
        curWidth = Math.min(img[0].orgWidth, 2 * $.BBCode.IMG_MaxWidth);
        hyperlink.text($.BBCode.IMG_MsgLargeToSmall);
    }

    img[0].width = curWidth;
    img.css("width", curWidth);
    img.css("max-width", curWidth);
    img.css("min-width", curWidth);

    hyperlink[0].width = curWidth;
    hyperlink.css("width", curWidth);
    hyperlink.css("max-width", curWidth);
    hyperlink.css("min-width", curWidth);
};
//________________________________________________________________________
