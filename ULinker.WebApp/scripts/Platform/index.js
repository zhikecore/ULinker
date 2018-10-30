function ShowModal(id, name,description) {
    if (id == 0) {
        //add
        $('#platformModal .modal-title').html('添加');
        $('#Id').val('');
        $('#Name2').val('');
        $('#Description').val('');

        $('#platformModal').modal('show');
    } else {
        //update
        $('#platformModal .modal-title').html('修改');

        $('#Id').val(id);
        $('#Name2').val(name);
        $('#Description').val(description);

        $('#platformModal').modal('show');
    }
}

function AddOrUpdate() {

    var id = $('#Id').val();
    if (id == 0) {
        Add();
    } else {
        Update();
    }
}

//添加
function Add() {
    var name = $('#Name2').val();
    var description = $('#Description').val();

    //1.validation
    if (name == '') {
        bootbox.alert("名称必填!");
        return;
    }

    //2.add
    $.ajax({
        url: '/Platform/Create',
        data: {
            name: name,
            description: description
        },
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            if (result.Bresult) {
                $('#platformModal').modal('hide');
                $("#platform_datatable").dataTable().fnFilter();
            }
            else {
                bootbox.alert(result.Notice);
            }
        },
        error: function (e) {
            console.log("Add异常:" + e.responseText);
        }
    });
}

//修改
function Update() {

    var id = $('#Id').val();
    var name = $('#Name2').val();
    var description = $('#Description').val();

    //1.validation
    if (name == '') {
        bootbox.alert("名称必填!");
        return;
    }

    //2.update
    $.ajax({
        url: '/Platform/Update',
        data: {
            id: id,
            name: name,
            description: description
        },
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            if (result.Bresult) {
                $('#platformModal').modal('hide');
                $("#platform_datatable").dataTable().fnFilter();
            }
            else {
                bootbox.alert(result.Notice);
            }
        },
        error: function (e) {
            console.log("Update异常:" + e.responseText);
        }
    });

}

function PhysicDelete(id) {

    $.ajax({
        url: '/Platform/PhysicDelete',
        data: {
            id: id
        },
        type: 'delete',
        cache: false,
        dataType: 'json',
        success: function (result) {
            bootbox.alert(result.Notice);
        },
        error: function (e) {
            console.log("Delete异常:" + e.responseText);
        }
    });
}