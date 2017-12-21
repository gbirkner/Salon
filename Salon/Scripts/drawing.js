// mouse position
var tempMouseX, tempMouseY, mouseX, mouseY, mouseDown = 0;

// get canvases and graphics contexts
var tempCanvas = document.getElementById('tempCanvas');
var tempCanvasContext = tempCanvas.getContext('2d');

var sketchCanvasFront = document.getElementById('sketchCanvasFront');
var sketchCanvasFrontContext = sketchCanvasFront.getContext('2d');

var sketchCanvasBack = document.getElementById('sketchCanvasBack');
var sketchCanvasBackContext = sketchCanvasBack.getContext('2d');

var sketchCanvasSide = document.getElementById('sketchCanvasSide');
var sketchCanvasSideContext = sketchCanvasSide.getContext('2d');

// graphics context currently drawn on
var drawContext = sketchCanvasFrontContext;

// buttons
var modeButton = document.getElementById('modeButton');
var frontViewButton = document.getElementById('frontViewButton');
var backViewButton = document.getElementById('backViewButton');
var sideViewButton = document.getElementById('sideViewButton');
var resetButton = document.getElementById('resetButton');
var undoButton = document.getElementById('undoButton');
var submitButton = document.getElementById('submitButton');

// backgrounds
var sketchBgFront = document.getElementById('sketchBgFront');
var sketchBgBack = document.getElementById('sketchBgBack');
var sketchBgSide = document.getElementById('sketchBgSide');

// submit images
var sketchDataFront = document.getElementById('sketchDataFront');
var sketchDataBack = document.getElementById('sketchDataBack');
var sketchDataSide = document.getElementById('sketchDataSide');

// description fields
var descriptionFront = document.getElementById('descriptionFront');
var descriptionBack = document.getElementById('descriptionBack');
var descriptionSide = document.getElementById('descriptionSide');

// drawing mode (1 = line, 2 = free)
var drawMode = 2;

// canvas view mode (1 = front, 2 = back, 3 = side)
var viewMode = 1;

// canvas histories
var sketchFrontHistory = [];
var sketchFrontCounter = 0;

var sketchBackHistory = [];
var sketchBackCounter = 0;

var sketchSideHistory = [];
var sketchSideCounter = 0;

// free draw mode history range
var freeDrawFirstIndex = -1;

// draw line
function drawLine(ctx, x1, y1, x2, y2) {

    // draw the line
    ctx.beginPath();
    ctx.moveTo(x1, y1);
    ctx.lineTo(x2, y2);
    ctx.stroke();
}

// mouse pressed
function mousePressed(e) {

    mouseDown = 1;
    getTempMousePos(e);

    // get free draw starting index
    if (drawMode == 2) {

        switch (viewMode) {

            case 1:
                freeDrawFirstIndex = sketchFrontCounter;
                break;

            case 2:
                freeDrawFirstIndex = sketchBackCounter;
                break;

            case 3:
                freeDrawFirstIndex = sketchSideCounter;
                break;
        }
    }
}

// mouse released
function mouseReleased() {

    if (mouseDown == 1) {

        mouseDown = 0;

        // draw line on sketch canvas
        if (drawMode == 1) {

            tempCanvasContext.clearRect(0, 0, tempCanvas.width, tempCanvas.height);
            drawLine(drawContext, tempMouseX, tempMouseY, mouseX, mouseY);

            // add history entry
            switch (viewMode) {

                case 1:
                    sketchFrontHistory[sketchFrontCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, -1];
                    sketchFrontCounter++;
                    break;

                case 2:
                    sketchBackHistory[sketchBackCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, -1];
                    sketchBackCounter++;
                    break;

                case 3:
                    sketchSideHistory[sketchSideCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, -1];
                    sketchSideCounter++;
                    break;
            }
        }
    }
}

// mouse moved
function mouseMoved(e) {

    // get mouse position
    getMousePos(e);

    // draw if mouse is down
    if (mouseDown == 1) {

        // draw line mode
        if (drawMode == 1) {

            tempCanvasContext.clearRect(0, 0, tempCanvas.width, tempCanvas.height);
            drawLine(tempCanvasContext, tempMouseX, tempMouseY, mouseX, mouseY);
        }

        // draw free mode
        if (drawMode == 2) {

            if (mouseX == tempMouseX && mouseY == tempMouseY)
                return;

            drawLine(drawContext, tempMouseX, tempMouseY, mouseX, mouseY);

            // add history entry
            switch (viewMode) {

                case 1:
                    sketchFrontHistory[sketchFrontCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, freeDrawFirstIndex];
                    sketchFrontCounter++;
                    break;

                case 2:
                    sketchBackHistory[sketchBackCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, freeDrawFirstIndex];
                    sketchBackCounter++;
                    break;

                case 3:
                    sketchSideHistory[sketchSideCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, freeDrawFirstIndex];
                    sketchSideCounter++;
                    break;
            }

            getTempMousePos(e);
        }
    }
}

// get current mouse position
function getMousePos(e) {

    if (!e)
        var e = event;

    if (e.offsetX) {

        mouseX = e.offsetX;
        mouseY = e.offsetY;

    } else if (e.layerX) {

        mouseX = e.layerX;
        mouseY = e.layerY;
    }
}

// get temp mouse position
function getTempMousePos(e) {

    if (!e)
        var e = event;

    if (e.offsetX) {

        tempMouseX = e.offsetX;
        tempMouseY = e.offsetY;

    } else if (e.layerX) {

        tempMouseX = e.layerX;
        tempMouseY = e.layerY;
    }
}

// switch mode button pressed
function modeButtonPressed(e) {

    if (drawMode == 2) {

        drawMode = 1;
        modeButton.innerHTML = 'Linie';

    } else if (drawMode == 1) {

        drawMode = 2;
        modeButton.innerHTML = 'Freihand';
    }
}

// change view
function viewButtonPressed(e) {

    if (e.target == frontViewButton) {

        viewMode = 1;
        sketchCanvasFront.style.visibility = 'visible';
        sketchCanvasBack.style.visibility = 'hidden';
        sketchCanvasSide.style.visibility = 'hidden';
        drawContext = sketchCanvasFrontContext;

        descriptionFront.style.visibility = 'visible';
        descriptionBack.style.visibility = 'hidden';
        descriptionSide.style.visibility = 'hidden';

    } else if (e.target == backViewButton) {

        viewMode = 2;
        sketchCanvasFront.style.visibility = 'hidden';
        sketchCanvasBack.style.visibility = 'visible';
        sketchCanvasSide.style.visibility = 'hidden';
        drawContext = sketchCanvasBackContext;

        descriptionFront.style.visibility = 'hidden';
        descriptionBack.style.visibility = 'visible';
        descriptionSide.style.visibility = 'hidden';

    } else if (e.target == sideViewButton) {

        viewMode = 3;
        sketchCanvasFront.style.visibility = 'hidden';
        sketchCanvasBack.style.visibility = 'hidden';
        sketchCanvasSide.style.visibility = 'visible';
        drawContext = sketchCanvasSideContext;

        descriptionFront.style.visibility = 'hidden';
        descriptionBack.style.visibility = 'hidden';
        descriptionSide.style.visibility = 'visible';
    }
}

// reset canvas
function resetCanvas() {

    switch (viewMode) {

        case 1:
            sketchCanvasFrontContext.clearRect(0, 0, sketchCanvasFront.width, sketchCanvasFront.height);
            sketchCanvasFrontContext.drawImage(sketchBgFront, 0, 0);
            break;

        case 2:
            sketchCanvasBackContext.clearRect(0, 0, sketchCanvasBack.width, sketchCanvasBack.height);
            sketchCanvasBackContext.drawImage(sketchBgBack, 0, 0);
            break;

        case 3:
            sketchCanvasSideContext.clearRect(0, 0, sketchCanvasSide.width, sketchCanvasSide.height);
            sketchCanvasSideContext.drawImage(sketchBgSide, 0, 0);
            break;
    }
}

// reset button
function resetButtonPressed(e) {

    resetCanvas();

    switch (viewMode) {

        case 1:
            sketchFrontCounter = 0;
            sketchFrontHistory = [];
            break;

        case 2:
            sketchBackCounter = 0;
            sketchBackHistory = [];
            break;

        case 3:
            sketchSideCounter = 0;
            sketchSideHistory = [];
            break;
    }
}

// revert one line on canvas
function undoButtonPressed(e) {

    resetCanvas();

    switch (viewMode) {

        case 1:
            if (sketchFrontCounter == 0)
                break;

            sketchFrontCounter--;
            
            // if last line is part of a free draw curve
            if (sketchFrontHistory[sketchFrontCounter][4] >= 0) {

                var startIndex = sketchFrontHistory[sketchFrontCounter][4];

                while (sketchFrontHistory[sketchFrontCounter][4] == startIndex && sketchFrontCounter > 0) {

                    sketchFrontCounter--;

                    if (sketchFrontHistory[sketchFrontCounter][4] != startIndex) {

                        sketchFrontCounter++;
                        break;
                    }
                }
            }

            if (sketchFrontCounter == 0)
                break;

            for (var i = 0; i < sketchFrontCounter; i++) {
                var x1 = sketchFrontHistory[i][0];
                var y1 = sketchFrontHistory[i][1];
                var x2 = sketchFrontHistory[i][2];
                var y2 = sketchFrontHistory[i][3];
                drawLine(drawContext, x1, y1, x2, y2);
            }
            break;

        case 2:
            if (sketchBackCounter == 0)
                break;

            sketchBackCounter--;

            // if last line is part of a free draw curve
            if (sketchBackHistory[sketchBackCounter][4] >= 0) {

                var startIndex = sketchBackHistory[sketchBackCounter][4];

                while (sketchBackHistory[sketchBackCounter][4] == startIndex && sketchBackCounter > 0) {

                    sketchBackCounter--;

                    if (sketchBackHistory[sketchBackCounter][4] != startIndex) {

                        sketchBackCounter++;
                        break;
                    }
                }
            }

            if (sketchBackCounter == 0)
                break;

            for (var i = 0; i < sketchBackCounter; i++) {
                var x1 = sketchBackHistory[i][0];
                var y1 = sketchBackHistory[i][1];
                var x2 = sketchBackHistory[i][2];
                var y2 = sketchBackHistory[i][3];
                drawLine(drawContext, x1, y1, x2, y2);
            }
            break;

        case 3:
            if (sketchSideCounter == 0)
                break;

            sketchSideCounter--;

            // if last line is part of a free draw curve
            if (sketchSideHistory[sketchSideCounter][4] >= 0) {

                var startIndex = sketchSideHistory[sketchSideCounter][4];

                while (sketchSideHistory[sketchSideCounter][4] == startIndex && sketchSideCounter > 0) {

                    sketchSideCounter--;

                    if (sketchSideHistory[sketchSideCounter][4] != startIndex) {

                        sketchSideCounter++;
                        break;
                    }
                }
            }

            if (sketchSideCounter == 0)
                break;

            for (var i = 0; i < sketchSideCounter; i++) {
                var x1 = sketchSideHistory[i][0];
                var y1 = sketchSideHistory[i][1];
                var x2 = sketchSideHistory[i][2];
                var y2 = sketchSideHistory[i][3];
                drawLine(drawContext, x1, y1, x2, y2);
            }
            break;
    }
}

// submit sketches
function submitButtonPressed(e) {

    // front view
    if (sketchFrontCounter > 0) {

        sketchDataFront.value = sketchCanvasFront.toDataURL('image/png').replace('data:image/png;base64,', '');
    }

    // back view
    if (sketchBackCounter > 0) {

        sketchDataBack.value = sketchCanvasBack.toDataURL('image/png').replace('data:image/png;base64,', '');
    }

    // side view
    if (sketchSideCounter > 0) {

        sketchDataSide.value = sketchCanvasSide.toDataURL('image/png').replace('data:image/png;base64,', '');
    }
}

// set canvas backgrounds
sketchCanvasFrontContext.drawImage(sketchBgFront, 0, 0);
sketchCanvasBackContext.drawImage(sketchBgBack, 0, 0);
sketchCanvasSideContext.drawImage(sketchBgSide, 0, 0);

// add event listeners
tempCanvas.addEventListener('mousedown', mousePressed);
tempCanvas.addEventListener('mousemove', mouseMoved);
window.addEventListener('mouseup', mouseReleased);
modeButton.addEventListener('click', modeButtonPressed);
frontViewButton.addEventListener('click', viewButtonPressed);
backViewButton.addEventListener('click', viewButtonPressed);
sideViewButton.addEventListener('click', viewButtonPressed);
resetButton.addEventListener('click', resetButtonPressed);
undoButton.addEventListener('click', undoButtonPressed);
submitButton.addEventListener('click', submitButtonPressed);