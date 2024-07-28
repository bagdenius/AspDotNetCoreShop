let dataTable;

$(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Company/GetAll' },
        "columns": [
            { data: 'name', width: "15%" },
            { data: 'phoneNumber', width: "13%" },
            { data: 'country', width: "10%" },
            { data: 'state', width: "10%" },
            { data: 'city', width: "10%" },
            { data: 'address', width: "14%" },
            { data: 'postalCode', width: "5%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="btn-group w-75" role="group">
                                <a href="/Admin/Company/Upsert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> Edit</a>
                                <a onClick=Delete('/Admin/Company/Delete/${data}') class="btn btn-danger mx-2"><i class="bi bi-trash-fill"></i> Delete</a>
                            </div>`
                },
                width: "23%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}