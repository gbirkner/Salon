// draw sketch modal
$.get("/Pictures/DrawSketch", function (response) {
    $('#addSketchModalBody').html(response);
    $('#sketchVisitId').val($('#btnSketch').val());
});

$("#btnSketch").on("click", function (e) {
    if ($('#btnSketch').val() != "0") {
        $("#addSketchModal").modal();
    } else {
        $("#warningModal").modal();
    }
});

// upload photo modal
$.get("/Pictures/AddPhoto", function (response) {
    $('#addPhotoModalBody').html(response);
    $('#photoVisitId').val($('#btnSketch').val());
});

$('#btnPhoto').on('click', function (e) {
    if ($('#btnPhoto').val() != "0") {
        $("#addPhotoModal").modal();
    } else {
        $("#warningModal").modal();
    }
});

// submit buttons
/*
$('#submitSketchButton').on('click', function (e) {
    $('#btn_quickSave').click();
});

$('#submitPhotoButton').on('click', function (e) {
    $('#btn_quickSave').click();
});*/