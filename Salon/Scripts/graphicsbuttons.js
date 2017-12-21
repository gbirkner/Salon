$('#btnSketch').on('click', function (e) {
    $.get("/Pictures/DrawSketch", function (response) {
        $('#addGraphicDiv').html(response);
    });
});

$('#btnPhoto').on('click', function (e) {
    $.get("/Pictures/AddPhoto", function (response) {
        $('#addGraphicDiv').html(response);
    });
});