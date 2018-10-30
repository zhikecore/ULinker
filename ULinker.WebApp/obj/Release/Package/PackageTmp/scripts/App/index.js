
//绑定产品
function bindUserCombox(id) {

    $.ajax({
        type: 'GET',
        url: '/User/GetUsersForCombox',
        data: null,
        async: false,
        success: function (json) {

            //根据id查找对象，
            var obj = document.getElementById(id);

            $.each(json, function (i, o) {
                //这个兼容IE与firefox
                obj.options.add(new Option(o.Name, o.Id));
            });

            if ($('#' + id + '').val() == null || $('#' + id + '').val() == '')
                $('#' + id + '').val(-1);
        }
    });
}

function ShowModal(id,appTypeId, managerId, name,description) {
    if (id == 0) {
        //add
        $('#appModal .modal-title').html('添加');
        $('#Id').val('');
        $('#dp_user2').val('');
        $('#dp_apptype2').val('');
        $('#Name2').val('');
        $('#Description').val('');

        $('#appModal').modal('show');
    } else {
        //update
        $('#appModal .modal-title').html('修改');

        $('#Id').val(id);
        $('#dp_user2').val(managerId);
        $('#dp_apptype2').val(appTypeId);
        $('#Name2').val(name);
        $('#Description').val(description);

        $('#appModal').modal('show');
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
    var appTypeId = $('#dp_apptype2').val();
    var managerId = $('#dp_user2').val();
    var name = $('#Name2').val();
    var description = $('#Description').val();

    //1.validation
    if (name == '') {
        bootbox.alert("名称必填!");
        return;
    }

    if (appTypeId == '' || appTypeId == 0) {
        bootbox.alert("程序类型必选!");
        return;
    }

    if (managerId == '' || managerId == 0) {
        bootbox.alert("负责人必选!");
        return;
    }

    //2.add
    $.ajax({
        url: '/App/Create',
        data: {
            appTypeId: appTypeId,
            managerId:managerId,
            name: name,
            description: description
        },
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            if (result.Bresult) {
                $('#appModal').modal('hide');
                $("#app_datatable").dataTable().fnFilter();
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
    var appTypeId = $('#dp_apptype2').val();
    var managerId = $('#dp_user2').val();
    var name = $('#Name2').val();
    var description = $('#Description').val();

    //1.validation
    if (name == '') {
        bootbox.alert("名称必填!");
        return;
    }

    if (appTypeId == '' || appTypeId == 0) {
        bootbox.alert("程序类型必选!");
        return;
    }

    if (managerId == '' || managerId == 0) {
        bootbox.alert("负责人必选!");
        return;
    }

    //2.update
    $.ajax({
        url: '/App/Update',
        data: {
            id: id,
            appTypeId: appTypeId,
            managerId: managerId,
            name: name,
            description: description
        },
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            if (result.Bresult) {
                $('#appModal').modal('hide');
                $("#app_datatable").dataTable().fnFilter();
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
        url: '/App/PhysicDelete',
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