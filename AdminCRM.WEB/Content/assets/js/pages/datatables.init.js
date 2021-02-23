$(document).ready(function () {
    $("#datatable").DataTable(
        {
            'paging': false,
            'info': false,
            'autoWidth': false,
            'ordering': false,
            'searching': false,
            'lengthChange': false
        }
    ),
        $("#datatable-buttons").DataTable({ lengthChange: !1, buttons: ["copy", "excel", "pdf", "colvis"] }).buttons().container().appendTo("#datatable-buttons_wrapper .col-md-6:eq(0)")
});