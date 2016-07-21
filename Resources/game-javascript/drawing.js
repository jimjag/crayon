﻿

if (!C$game) { throw 1; } // Cannot use the drawing library without the game library.

// Lookups for fast hex conversion
C$drawing = 1;
C$drawing$HEX = [];
C$drawing$HEXR = [];
C$drawing$events = [];
C$drawing$eventsLength = 0;
C$drawing$images = [];

for (var i = 0; i < 256; ++i) {
    var t = i.toString(16);
    if (i < 16) t = '0' + t;
    C$drawing$HEX.push(t);
    C$drawing$HEXR.push('#' + t);
}

C$drawing$rendererSetData = function (events, eventsLength, images) {
    C$drawing$events = events;
    C$drawing$eventsLength = eventsLength;
    C$drawing$images = images;
    C$drawing$render();
};

C$drawing$render = function () {
    var ev = C$drawing$events;
    var images = C$drawing$images;
    var imagesIndex = 0;
    var canvas = null;
    var mask = 0;
    var x = 0;
    var y = 0;
    var w = 0;
    var h = 0;
    var r = 0;
    var g = 0;
    var b = 0;
    var a = 0;
    var tw = 0;
    var th = 0;
    var sx = 0;
    var sy = 0;
    var sw = 0;
    var sh = 0;
    var alpha = 0;
    var theta = 0;
    var radiusX = 0;
    var radiusY = 0;
    for (var i = 0; i < C$drawing$eventsLength; i += 16) {
        switch (ev[i]) {
            case 1:
                // rectangle
                x = ev[i | 1];
                y = ev[i | 2];
                w = ev[i | 3];
                h = ev[i | 4];
                r = ev[i | 5];
                g = ev[i | 6];
                b = ev[i | 7];
                a = ev[i | 8];

                C$game$ctx.fillStyle = C$drawing$HEXR[r] + C$drawing$HEX[g] + C$drawing$HEX[b];
                if (a != 255) {
                    C$game$ctx.globalAlpha = a / 255.0;
                    C$game$ctx.fillRect(x, y, w + .1, h + .1); // TODO: get to the bottom of this mysterious .1. Is it still necessary?
                    C$game$ctx.globalAlpha = 1;
                } else {
                    C$game$ctx.fillRect(x, y, w + .1, h + .1);
                }
                break;

            case 2:
                // ellipse
                w = ev[i | 3] / 2; // note that w and h are half width and half height
                h = ev[i | 4] / 2;
                x = ev[i | 1] + w;
                y = ev[i | 2] + h;
                r = ev[i | 5];
                g = ev[i | 6];
                b = ev[i | 7];
                a = ev[i | 8];

                w = w * 4 / 3; // no idea why this needs to exist to look correct...
                C$game$ctx.beginPath();
                C$game$ctx.moveTo(x, y - h);
                C$game$ctx.bezierCurveTo(
                    x + w, y - h,
                    x + w, y + h,
                    x, y + h);
                C$game$ctx.bezierCurveTo(
                    x - w, y + h,
                    x - w, y - h,
                    x, y - h);
                C$game$ctx.fillStyle = C$drawing$HEXR[r] + C$drawing$HEX[g] + C$drawing$HEX[b];
                if (a != 255) {
                    C$game$ctx.globalAlpha = a / 255.0;
                    C$game$ctx.fill();
                    C$game$ctx.closePath();
                    C$game$ctx.globalAlpha = 1;
                } else {
                    C$game$ctx.fill();
                    C$game$ctx.closePath();
                }
                break;
            case 3:
                // line
                break;
            case 4:
                // triangle
                break;
            case 5:
                // images
                canvas = images[imagesIndex++][0][1];
                x = ev[i | 8];
                y = ev[i | 9];
                w = canvas.width;
                h = canvas.height;
                mask = ev[i | 1];
                if (mask == 0) {
                    // basic case
                    C$game$ctx.drawImage(canvas, 0, 0, w, h, x, y, w, h);
                } else if ((mask & 4) != 0) {
                    // rotation is involved
                    theta = ev[i | 10] / 1048576.0;
                    if ((mask & 3) == 0) {
                        C$game$ctx.save();
                        C$game$ctx.translate(x, y);
                        C$game$ctx.rotate(theta);

                        if ((mask & 8) == 0) {
                            C$game$ctx.drawImage(canvas, -w / 2, -h / 2);
                        } else {
                            C$game$ctx.globalAlpha = ev[i | 11] / 255;
                            C$game$ctx.drawImage(canvas, -w / 2, -h / 2);
                            C$game$ctx.globalAlpha = 1;
                        }
                        C$game$ctx.restore();
                    } else {
                        // TODO: slice and scale a picture and rotate it.
                    }
                } else {
                    // no rotation
                    if ((mask & 1) == 0) {
                        sx = 0;
                        sy = 0;
                        sw = w;
                        sh = h;
                    } else {
                        sx = ev[i | 2];
                        sy = ev[i | 3];
                        sw = ev[i | 4];
                        sh = ev[i | 5];
                    }
                    if ((mask & 2) == 0) {
                        tw = sw;
                        th = sh;
                    } else {
                        tw = ev[i | 6];
                        th = ev[i | 7];
                    }

                    if ((mask & 8) == 0) {
                        C$game$ctx.drawImage(canvas, sx, sy, sw, sh, x, y, tw, th);
                    } else {
                        C$game$ctx.globalAlpha = ev[i | 11] / 255;
                        C$game$ctx.drawImage(canvas, sx, sy, sw, sh, x, y, tw, th);
                        C$game$ctx.globalAlpha = 1;
                    }
                }
                break;
        }

    }
};

C$drawing$blitRotated = function (canvas, x, y, theta) {
    C$game$ctx.save();
    C$game$ctx.translate(x, y);
    C$game$ctx.rotate(theta);
    C$game$ctx.drawImage(canvas, -canvas.width / 2, -canvas.height / 2);
    C$game$ctx.restore();
};

C$drawing$blitPartial = function (canvas, tx, ty, tw, th, sx, sy, sw, sh) {
	if (tw == 0 || th == 0 || sw == 0 || sh == 0) return;

	C$game$ctx.drawImage(canvas, sx, sy, sw, sh, tx, ty, tw, th);
};

C$drawing$drawImageWithAlpha = function (canvas, x, y, a) {
	if (a == 0) return;
	if (a != 255) {
	    C$game$ctx.globalAlpha = a / 255;
	    C$game$ctx.drawImage(canvas, 0, 0, canvas.width, canvas.height, x, y, canvas.width, canvas.height);
	    C$game$ctx.globalAlpha = 1;
	} else {
	    C$game$ctx.drawImage(canvas, 0, 0, canvas.width, canvas.height, x, y, canvas.width, canvas.height);
	}
};

C$drawing$fillScreen = function (r, g, b) {
    C$game$ctx.fillStyle = C$drawing$HEXR[r] + C$drawing$HEX[g] + C$drawing$HEX[b];
    C$game$ctx.fillRect(0, 0, C$game$width, C$game$height);
};

C$drawing$drawRect = function (x, y, width, height, r, g, b, a) {
    C$game$ctx.fillStyle = C$drawing$HEXR[r] + C$drawing$HEX[g] + C$drawing$HEX[b];
	if (a != 255) {
	    C$game$ctx.globalAlpha = a / 255;
	    C$game$ctx.fillRect(x, y, width + .1, height + .1);
	    C$game$ctx.globalAlpha = 1;
	} else {
	    C$game$ctx.fillRect(x, y, width + .1, height + .1);
	}
};

C$drawing$drawTriangle = function (ax, ay, bx, by, cx, cy, r, g, b, a) {
	if (a == 0) return;

	var tpath = new Path2D();
	tpath.moveTo(ax, ay);
	tpath.lineTo(bx, by);
	tpath.lineTo(cx, cy);

	C$game$ctx.fillStyle = C$drawing$HEXR[r] + C$drawing$HEX[g] + C$drawing$HEX[b];
	if (a != 255) {
	    C$game$ctx.globalAlpha = a / 255;
	    C$game$ctx.fill(tpath);
	    C$game$ctx.globalAlpha = 1;
	} else {
	    C$game$ctx.fill(tpath);
	}
};

C$drawing$drawEllipse = function (left, top, width, height, r, g, b, alpha) {
	var radiusX = width / 2;
	var radiusY = height / 2;
	var centerX = left + radiusX;
	var centerY = top + radiusY;

	radiusX = radiusX * 4 / 3; // no idea...
	C$game$ctx.beginPath();
	C$game$ctx.moveTo(centerX, centerY - radiusY);
	C$game$ctx.bezierCurveTo(
		centerX + radiusX, centerY - radiusY,
		centerX + radiusX, centerY + radiusY,
		centerX, centerY + radiusY);
	C$game$ctx.bezierCurveTo(
		centerX - radiusX, centerY + radiusY,
		centerX - radiusX, centerY - radiusY,
		centerX, centerY - radiusY);
	C$game$ctx.fillStyle = C$drawing$HEXR[r] + C$drawing$HEX[g] + C$drawing$HEX[b];
	if (alpha != 255) {
	    C$game$ctx.globalAlpha = alpha / 255;
	    C$game$ctx.fill();
	    C$game$ctx.closePath();
	    C$game$ctx.globalAlpha = 1;
	} else {
	    C$game$ctx.fill();
	    C$game$ctx.closePath();
	}
};

C$drawing$drawLine = function (startX, startY, endX, endY, width, r, g, b, a) {
	var offset = ((width % 2) == 0) ? 0 : .5;
	C$game$ctx.beginPath();
	C$game$ctx.moveTo(startX + offset, startY + offset);
	C$game$ctx.lineTo(endX + offset, endY + offset);
	C$game$ctx.lineWidth = width;
	if (a != 255) {
	    C$game$ctx.globalAlpha = a / 255;
	    C$game$ctx.strokeStyle = C$drawing$HEXR[r] + C$drawing$HEX[g] + C$drawing$HEX[b];
	    C$game$ctx.stroke();
	    C$game$ctx.closePath();
	    C$game$ctx.globalAlpha = 1;
	} else {
	    C$game$ctx.strokeStyle = C$drawing$HEXR[r] + C$drawing$HEX[g] + C$drawing$HEX[b];
	    C$game$ctx.stroke();
	    C$game$ctx.closePath();
	}
};

C$drawing$flipImage = function (canvas, flipX, flipY) {
	var output = document.createElement('canvas');

	output.width = canvas.width;
	output.height = canvas.height;

	var ctx = output.getContext('2d');

	if (flipX) {
	    ctx.translate(canvas.width, 0);
	    ctx.scale(-1, 1);
	}
	if (flipY) {
	    ctx.translate(0, canvas.height);
	    ctx.scale(1, -1);
	}

	ctx.drawImage(canvas, 0, 0);

	if (flipX) {
	    ctx.scale(-1, 1);
	    ctx.translate(-canvas.width, 0);
	}
	if (flipY) {
	    ctx.scale(1, -1);
	    ctx.translate(0, -canvas.height);
	}

	return output;
};

C$drawing$scaleImage = function (originalCanvas, width, height) {
	var output = document.createElement('canvas');
	var ctx = output.getContext('2d');
	output.width = width;
	output.height = height;
	ctx.webkitImageSmoothingEnabled = false;
	ctx.mozImageSmoothingEnabled = false;
	ctx.msImageSmoothingEnabled = false;
	ctx.imageSmoothingEnabled = false;
	ctx.drawImage(
		originalCanvas,
		0, 0, originalCanvas.width, originalCanvas.height,
		0, 0, width, height);
	return output;
};
