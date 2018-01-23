// mouse position
var tempMouseX, tempMouseY, mouseX, mouseY, mouseDown = 0;

// get canvases and graphics contexts
var tempCanvas        = document.getElementById('tempCanvas');
var tempCanvasContext = tempCanvas.getContext('2d');

var sketchCanvasDreieck = document.getElementById('sketchCanvasDreieck');
var sketchCanvasOval    = document.getElementById('sketchCanvasOval');
var sketchCanvasRund    = document.getElementById('sketchCanvasRund');
var sketchCanvasViereck = document.getElementById('sketchCanvasViereck');
var sketchCanvasSchmal  = document.getElementById('sketchCanvasSchmal');
var sketchCanvasSeite   = document.getElementById('sketchCanvasSeite');

var sketchCanvasDreieckContext = sketchCanvasDreieck.getContext('2d');
var sketchCanvasOvalContext    = sketchCanvasOval.getContext('2d');
var sketchCanvasRundContext    = sketchCanvasRund.getContext('2d');
var sketchCanvasViereckContext = sketchCanvasViereck.getContext('2d');
var sketchCanvasSchmalContext  = sketchCanvasSchmal.getContext('2d');
var sketchCanvasSeiteContext   = sketchCanvasSeite.getContext('2d');

// graphics context currently drawn on
var drawContext = sketchCanvasDreieckContext;

// buttons
var dreieckViewButton = document.getElementById('dreieckViewButton');
var ovalViewButton    = document.getElementById('ovalViewButton');
var rundViewButton    = document.getElementById('rundViewButton');
var viereckViewButton = document.getElementById('viereckViewButton');
var schmalViewButton  = document.getElementById('schmalViewButton');
var seiteViewButton   = document.getElementById('seiteViewButton');

var modeButton   = document.getElementById('modeButton');
var resetButton  = document.getElementById('resetButton');
var undoButton   = document.getElementById('undoButton');
var submitButton = document.getElementById('submitSketchButton');

// backgrounds
var sketchBgDreieck = document.getElementById('sketchBgDreieck');
var sketchBgOval    = document.getElementById('sketchBgOval');
var sketchBgRund    = document.getElementById('sketchBgRund');
var sketchBgViereck = document.getElementById('sketchBgViereck');
var sketchBgSchmal  = document.getElementById('sketchBgSchmal');
var sketchBgSeite   = document.getElementById('sketchBgSeite');

// submit images
var sketchDataDreieck = document.getElementById('sketchDataDreieck');
var sketchDataOval    = document.getElementById('sketchDataOval');
var sketchDataRund    = document.getElementById('sketchDataRund');
var sketchDataViereck = document.getElementById('sketchDataViereck');
var sketchDataSchmal  = document.getElementById('sketchDataSchmal');
var sketchDataSeite   = document.getElementById('sketchDataSeite');

// description fields
var descriptionDreieck = document.getElementById('descriptionDreieck');
var descriptionOval    = document.getElementById('descriptionOval');
var descriptionRund    = document.getElementById('descriptionRund');
var descriptionViereck = document.getElementById('descriptionViereck');
var descriptionSchmal  = document.getElementById('descriptionSchmal');
var descriptionSeite   = document.getElementById('descriptionSeite');

// drawing mode (1=line, 2=free)
var drawMode = 2;

// canvas view mode (1=dreieck, 2=oval, 3=rund, 4=viereck, 5=schmal, 6=seite)
var viewMode = 1;

// canvas histories
var sketchDreieckHistory = [];
var sketchOvalHistory    = [];
var sketchRundHistory    = [];
var sketchViereckHistory = [];
var sketchSchmalHistory  = [];
var sketchSeiteHistory   = [];

var sketchDreieckCounter = 0;
var sketchOvalCounter    = 0;
var sketchRundCounter    = 0;
var sketchViereckCounter = 0;
var sketchSchmalCounter  = 0;
var sketchSeiteCounter   = 0;

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
                freeDrawFirstIndex = sketchDreieckCounter;
                break;

            case 2:
                freeDrawFirstIndex = sketchOvalCounter;
                break;

            case 3:
                freeDrawFirstIndex = sketchRundCounter;
                break;

            case 4:
                freeDrawFirstIndex = sketchViereckCounter;
                break;

            case 5:
                freeDrawFirstIndex = sketchSchmalCounter;
                break;

            case 6:
                freeDrawFirstIndex = sketchSeiteCounter;
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
                    sketchDreieckHistory[sketchDreieckCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, -1];
                    sketchDreieckCounter++;
                    break;

                case 2:
                    sketchOvalHistory[sketchOvalCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, -1];
                    sketchOvalCounter++;
                    break;

                case 3:
                    sketchRundHistory[sketchRundCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, -1];
                    sketchRundCounter++;
                    break;

                case 4:
                    sketchViereckHistory[sketchViereckCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, -1];
                    sketchViereckCounter++;
                    break;

                case 5:
                    sketchSchmalHistory[sketchSchmalCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, -1];
                    sketchSchmalCounter++;
                    break;

                case 6:
                    sketchSeiteHistory[sketchSeiteCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, -1];
                    sketchSeiteCounter++;
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
                    sketchDreieckHistory[sketchDreieckCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, freeDrawFirstIndex];
                    sketchDreieckCounter++;
                    break;

                case 2:
                    sketchOvalHistory[sketchOvalCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, freeDrawFirstIndex];
                    sketchOvalCounter++;
                    break;

                case 3:
                    sketchRundHistory[sketchRundCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, freeDrawFirstIndex];
                    sketchRundCounter++;
                    break;

                case 4:
                    sketchViereckHistory[sketchViereckCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, freeDrawFirstIndex];
                    sketchViereckCounter++;
                    break;

                case 5:
                    sketchSchmalHistory[sketchSchmalCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, freeDrawFirstIndex];
                    sketchSchmalCounter++;
                    break;

                case 6:
                    sketchSeiteHistory[sketchSeiteCounter] = [tempMouseX, tempMouseY, mouseX, mouseY, freeDrawFirstIndex];
                    sketchSeiteCounter++;
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
        modeButton.innerHTML = '<span class="glyphicon glyphicon-pencil"></span> Linie';

    } else if (drawMode == 1) {

        drawMode = 2;
        modeButton.innerHTML = '<span class="glyphicon glyphicon-pencil"></span> Freihand';
    }
}

// change view
function viewButtonPressed(e) {

    sketchCanvasDreieck.style.display = 'none';
    sketchCanvasOval.style.display    = 'none';
    sketchCanvasRund.style.display    = 'none';
    sketchCanvasViereck.style.display = 'none';
    sketchCanvasSchmal.style.display  = 'none';
    sketchCanvasSeite.style.display   = 'none';

    descriptionDreieck.style.display = 'none';
    descriptionOval.style.display    = 'none';
    descriptionRund.style.display    = 'none';
    descriptionViereck.style.display = 'none';
    descriptionSchmal.style.display  = 'none';
    descriptionSeite.style.display   = 'none';

    dreieckViewButton.className = 'btn btn-default';
    ovalViewButton.className    = 'btn btn-default';
    rundViewButton.className    = 'btn btn-default';
    viereckViewButton.className = 'btn btn-default';
    schmalViewButton.className  = 'btn btn-default';
    seiteViewButton.className   = 'btn btn-default';

    if (e.target == dreieckViewButton) {

        viewMode = 1;
        sketchCanvasDreieck.style.display = 'block';
        drawContext = sketchCanvasDreieckContext;
        descriptionDreieck.style.display = 'block';
        dreieckViewButton.className = 'btn btn-primary';

    } else if (e.target == ovalViewButton) {

        viewMode = 2;
        sketchCanvasOval.style.display = 'block';
        drawContext = sketchCanvasOvalContext;
        descriptionOval.style.display = 'block';
        ovalViewButton.className = 'btn btn-primary';

    } else if (e.target == rundViewButton) {

        viewMode = 3;
        sketchCanvasRund.style.display = 'block';
        drawContext = sketchCanvasRundContext;
        descriptionRund.style.display = 'block';
        rundViewButton.className = 'btn btn-primary';

    } else if (e.target == viereckViewButton) {

        viewMode = 4;
        sketchCanvasViereck.style.display = 'block';
        drawContext = sketchCanvasViereckContext;
        descriptionViereck.style.display = 'block';
        viereckViewButton.className = 'btn btn-primary';

    } else if (e.target == schmalViewButton) {

        viewMode = 5;
        sketchCanvasSchmal.style.display = 'block';
        drawContext = sketchCanvasSchmalContext;
        descriptionSchmal.style.display = 'block';
        schmalViewButton.className = 'btn btn-primary';

    } else if (e.target == seiteViewButton) {

        viewMode = 6;
        sketchCanvasSeite.style.display = 'block';
        drawContext = sketchCanvasSeiteContext;
        descriptionSeite.style.display = 'block';
        seiteViewButton.className = 'btn btn-primary';
    }
}

// reset canvas
function resetCanvas() {

    switch (viewMode) {

        case 1:
            sketchCanvasDreieckContext.clearRect(0, 0, sketchCanvasDreieck.width, sketchCanvasDreieck.height);
            sketchCanvasDreieckContext.drawImage(sketchBgDreieck, 0, 0);
            break;

        case 2:
            sketchCanvasOvalContext.clearRect(0, 0, sketchCanvasOval.width, sketchCanvasOval.height);
            sketchCanvasOvalContext.drawImage(sketchBgOval, 0, 0);
            break;

        case 3:
            sketchCanvasRundContext.clearRect(0, 0, sketchCanvasRund.width, sketchCanvasRund.height);
            sketchCanvasRundContext.drawImage(sketchBgRund, 0, 0);
            break;

        case 4:
            sketchCanvasViereckContext.clearRect(0, 0, sketchCanvasViereck.width, sketchCanvasViereck.height);
            sketchCanvasViereckContext.drawImage(sketchBgViereck, 0, 0);
            break;

        case 5:
            sketchCanvasSchmalContext.clearRect(0, 0, sketchCanvasSchmal.width, sketchCanvasSchmal.height);
            sketchCanvasSchmalContext.drawImage(sketchBgSchmal, 0, 0);
            break;

        case 6:
            sketchCanvasSeiteContext.clearRect(0, 0, sketchCanvasSeite.width, sketchCanvasSeite.height);
            sketchCanvasSeiteContext.drawImage(sketchBgSeite, 0, 0);
            break;
    }
}

// reset button
function resetButtonPressed(e) {

    resetCanvas();

    switch (viewMode) {

        case 1:
            sketchDreieckCounter = 0;
            sketchDreieckHistory = [];
            break;

        case 2:
            sketchOvalCounter = 0;
            sketchOvalHistory = [];
            break;

        case 3:
            sketchRundCounter = 0;
            sketchRundHistory = [];
            break;

        case 4:
            sketchViereckCounter = 0;
            sketchViereckHistory = [];
            break;

        case 5:
            sketchSchmalCounter = 0;
            sketchSchmalHistory = [];
            break;

        case 6:
            sketchSeiteCounter = 0;
            sketchSeiteHistory = [];
            break;
    }
}

// revert one line on canvas
function undoButtonPressed(e) {

    resetCanvas();

    switch (viewMode) {

        case 1:
            if (sketchDreieckCounter == 0)
                break;

            sketchDreieckCounter--;
            
            // if last line is part of a free draw curve
            if (sketchDreieckHistory[sketchDreieckCounter][4] >= 0) {

                var startIndex = sketchDreieckHistory[sketchDreieckCounter][4];

                while (sketchDreieckHistory[sketchDreieckCounter][4] == startIndex && sketchDreieckCounter > 0) {

                    sketchDreieckCounter--;

                    if (sketchDreieckHistory[sketchDreieckCounter][4] != startIndex) {

                        sketchDreieckCounter++;
                        break;
                    }
                }
            }

            if (sketchDreieckCounter == 0)
                break;

            for (var i = 0; i < sketchDreieckCounter; i++) {
                var x1 = sketchDreieckHistory[i][0];
                var y1 = sketchDreieckHistory[i][1];
                var x2 = sketchDreieckHistory[i][2];
                var y2 = sketchDreieckHistory[i][3];
                drawLine(drawContext, x1, y1, x2, y2);
            }
            break;

        case 2:
            if (sketchOvalCounter == 0)
                break;

            sketchOvalCounter--;

            // if last line is part of a free draw curve
            if (sketchOvalHistory[sketchOvalCounter][4] >= 0) {

                var startIndex = sketchOvalHistory[sketchOvalCounter][4];

                while (sketchOvalHistory[sketchOvalCounter][4] == startIndex && sketchOvalCounter > 0) {

                    sketchOvalCounter--;

                    if (sketchOvalHistory[sketchOvalCounter][4] != startIndex) {

                        sketchOvalCounter++;
                        break;
                    }
                }
            }

            if (sketchOvalCounter == 0)
                break;

            for (var i = 0; i < sketchOvalCounter; i++) {
                var x1 = sketchOvalHistory[i][0];
                var y1 = sketchOvalHistory[i][1];
                var x2 = sketchOvalHistory[i][2];
                var y2 = sketchOvalHistory[i][3];
                drawLine(drawContext, x1, y1, x2, y2);
            }
            break;

        case 3:
            if (sketchRundCounter == 0)
                break;

            sketchRundCounter--;

            // if last line is part of a free draw curve
            if (sketchRundHistory[sketchRundCounter][4] >= 0) {

                var startIndex = sketchRundHistory[sketchRundCounter][4];

                while (sketchRundHistory[sketchRundCounter][4] == startIndex && sketchRundCounter > 0) {

                    sketchRundCounter--;

                    if (sketchRundHistory[sketchRundCounter][4] != startIndex) {

                        sketchRundCounter++;
                        break;
                    }
                }
            }

            if (sketchRundCounter == 0)
                break;

            for (var i = 0; i < sketchRundCounter; i++) {
                var x1 = sketchRundHistory[i][0];
                var y1 = sketchRundHistory[i][1];
                var x2 = sketchRundHistory[i][2];
                var y2 = sketchRundHistory[i][3];
                drawLine(drawContext, x1, y1, x2, y2);
            }
            break;

        case 4:
            if (sketchViereckCounter == 0)
                break;

            sketchViereckCounter--;

            // if last line is part of a free draw curve
            if (sketchViereckHistory[sketchViereckCounter][4] >= 0) {

                var startIndex = sketchViereckHistory[sketchViereckCounter][4];

                while (sketchViereckHistory[sketchViereckCounter][4] == startIndex && sketchViereckCounter > 0) {

                    sketchViereckCounter--;

                    if (sketchViereckHistory[sketchViereckCounter][4] != startIndex) {

                        sketchViereckCounter++;
                        break;
                    }
                }
            }

            if (sketchViereckCounter == 0)
                break;

            for (var i = 0; i < sketchViereckCounter; i++) {
                var x1 = sketchViereckHistory[i][0];
                var y1 = sketchViereckHistory[i][1];
                var x2 = sketchViereckHistory[i][2];
                var y2 = sketchViereckHistory[i][3];
                drawLine(drawContext, x1, y1, x2, y2);
            }
            break;

        case 5:
            if (sketchSchmalCounter == 0)
                break;

            sketchSchmalCounter--;

            // if last line is part of a free draw curve
            if (sketchSchmalHistory[sketchSchmalCounter][4] >= 0) {

                var startIndex = sketchSchmalHistory[sketchSchmalCounter][4];

                while (sketchSchmalHistory[sketchSchmalCounter][4] == startIndex && sketchSchmalCounter > 0) {

                    sketchSchmalCounter--;

                    if (sketchSchmalHistory[sketchSchmalCounter][4] != startIndex) {

                        sketchSchmalCounter++;
                        break;
                    }
                }
            }

            if (sketchSchmalCounter == 0)
                break;

            for (var i = 0; i < sketchSchmalCounter; i++) {
                var x1 = sketchSchmalHistory[i][0];
                var y1 = sketchSchmalHistory[i][1];
                var x2 = sketchSchmalHistory[i][2];
                var y2 = sketchSchmalHistory[i][3];
                drawLine(drawContext, x1, y1, x2, y2);
            }
            break;

        case 6:
            if (sketchSeiteCounter == 0)
                break;

            sketchSeiteCounter--;

            // if last line is part of a free draw curve
            if (sketchSeiteHistory[sketchSeiteCounter][4] >= 0) {

                var startIndex = sketchSeiteHistory[sketchSeiteCounter][4];

                while (sketchSeiteHistory[sketchSeiteCounter][4] == startIndex && sketchSeiteCounter > 0) {

                    sketchSeiteCounter--;

                    if (sketchSeiteHistory[sketchSeiteCounter][4] != startIndex) {

                        sketchSeiteCounter++;
                        break;
                    }
                }
            }

            if (sketchSeiteCounter == 0)
                break;

            for (var i = 0; i < sketchSeiteCounter; i++) {
                var x1 = sketchSeiteHistory[i][0];
                var y1 = sketchSeiteHistory[i][1];
                var x2 = sketchSeiteHistory[i][2];
                var y2 = sketchSeiteHistory[i][3];
                drawLine(drawContext, x1, y1, x2, y2);
            }
            break;
    }
}

// submit sketches
function submitButtonPressed(e) {

    if (sketchDreieckCounter > 0)
        sketchDataDreieck.value = sketchCanvasDreieck.toDataURL('image/png').replace('data:image/png;base64,', '');

    if (sketchOvalCounter > 0)
        sketchDataOval.value = sketchCanvasOval.toDataURL('image/png').replace('data:image/png;base64,', '');

    if (sketchRundCounter > 0)
        sketchDataRund.value = sketchCanvasRund.toDataURL('image/png').replace('data:image/png;base64,', '');

    if (sketchViereckCounter > 0)
        sketchDataViereck.value = sketchCanvasViereck.toDataURL('image/png').replace('data:image/png;base64,', '');

    if (sketchSchmalCounter > 0)
        sketchDataSchmal.value = sketchCanvasSchmal.toDataURL('image/png').replace('data:image/png;base64,', '');

    if (sketchSeiteCounter > 0)
        sketchDataSeite.value = sketchCanvasSeite.toDataURL('image/png').replace('data:image/png;base64,', '');
}

// set canvas backgrounds
sketchCanvasDreieckContext.drawImage(sketchBgDreieck, 0, 0);
sketchCanvasOvalContext.drawImage(sketchBgOval, 0, 0);
sketchCanvasRundContext.drawImage(sketchBgRund, 0, 0);
sketchCanvasViereckContext.drawImage(sketchBgViereck, 0, 0);
sketchCanvasSchmalContext.drawImage(sketchBgSchmal, 0, 0);
sketchCanvasSeiteContext.drawImage(sketchBgSeite, 0, 0);

// add event listeners
dreieckViewButton.addEventListener('click', viewButtonPressed);
ovalViewButton.addEventListener('click', viewButtonPressed);
rundViewButton.addEventListener('click', viewButtonPressed);
viereckViewButton.addEventListener('click', viewButtonPressed);
schmalViewButton.addEventListener('click', viewButtonPressed);
seiteViewButton.addEventListener('click', viewButtonPressed);

tempCanvas.addEventListener('mousedown', mousePressed);
tempCanvas.addEventListener('mousemove', mouseMoved);
window.addEventListener('mouseup', mouseReleased);

modeButton.addEventListener('click', modeButtonPressed);
resetButton.addEventListener('click', resetButtonPressed);
undoButton.addEventListener('click', undoButtonPressed);
submitButton.addEventListener('click', submitButtonPressed);