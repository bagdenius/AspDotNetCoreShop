let dataTable;

$(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else if (url.includes("pending")) {
        loadDataTable("pending");
    }
    else if (url.includes("completed")) {
        loadDataTable("completed");
    }
    else if (url.includes("approved")) {
        loadDataTable("approved");
    }
    else {
        loadDataTable("all");
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Order/GetAll?status=' + status },
        "columns": [
            { data: 'id', width: "0%" },
            { data: 'name', width: "0%" },
            { data: 'surname', width: "0%" },
            { data: 'phoneNumber', width: "0%" },
            { data: 'user.email', width: "0%" },
            { data: 'status', width: "0%" },
            { data: 'total', width: "0%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="btn-group w-75" role="group">
                                <a href="/Admin/Order/Details?id=${data}" class="btn btn-primary mx-2">Details</a>
                            </div>`
                },
                width: "0%"
            }
        ]
    });
}