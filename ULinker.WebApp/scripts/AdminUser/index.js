
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

function ShowModal(flag,id, realname, phone, description) {

    //add
    if (flag == 0) {
        $('#userModal .modal-title').html('添加');
        $('#Id').val('');
        $('#RealName').val('');
        $('#Phone').val('');
        $('#Description').val('');
        $('#flag').val(flag);
    } else {
        //update
        $('#userModal .modal-title').html('修改');
        $('#Id').val(id);
        $('#RealName').val(realname);
        $('#Phone').val(phone);
        $('#Description').val(description);
        $('#flag').val(flag);
    }

    $('#userModal').modal('show');
}


function ShowDetail(id)
{
    $.ajax({
        url: '/AdminUser/GetById',
        data: {
            id: id
        },
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (data) {
            //update
            $('#userModal .modal-title').html('明细');

            $('#Id').val(data.Id);
            $('#RealName').val(data.RealName);
            $('#Phone').val(data.Phone);
            $('#Description').val(data.Description);

            $('#userModal').modal('show');
        },
        error: function (e) {
            console.log("Add异常:" + e.responseText);
        }
    });
}

function AddOrUpdate()
{
    var flag = $('#flag').val();
    switch(flag)
    {
        case "0":
            Add();
            break;
        case "1":
            Update();
            break;
    }
}

//添加
function Add() {
    //var appTypeId = $('#dp_apptype2').val();
    //var managerId = $('#dp_user2').val();
    var id = $('#Id').val();
    var realname = $('#RealName').val();
    var phone = $('#Phone').val();
    var description = $('#Description').val();

    //1.validation
    if (id == '' || id == undefined || id==null) {
        bootbox.alert("Id必填!");
        return;
    }

    if (realname == '') {
        bootbox.alert("姓名必填!");
        return;
    }

    if (phone == '') {
        bootbox.alert("电话必填!");
        return;
    }

    //2.add
    $.ajax({
        url: '/AdminUser/Create',
        data: {
            id:id,
            realname: realname,
            phone:phone,
            description: description
        },
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            if (result.Result) {
                $('#userModal').modal('hide');
                $("#user_datatable").dataTable().fnFilter();
            }
            else {
                bootbox.alert(result.Message);
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
    var phone = $('#Phone').val();
    var realname = $('#RealName').val();
    var description = $('#Description').val();

    //1.validation
    if (realname == '') {
        bootbox.alert("名称必填!");
        return;
    }

    //2.update
    $.ajax({
        url: '/AdminUser/Update',
        data: {
            id: id,
            realname: realname,
            description: description
        },
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            if (result.Bresult) {
                $('#userModal').modal('hide');
                $("#user_datatable").dataTable().fnFilter();
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

function PhysicDelete(id)
{
    bootbox.confirm({
        message: "您确定要删除吗?id为"+id+"的用户吗?",
        buttons: {
            confirm: {
                label: '确定',
                className: 'btn-success'
            },
            cancel: {
                label: '取消',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result)
            {
                _Delete(id);
            }
        }
    });
    
}

function _Delete(id) {
    $.ajax({
        url: '/AdminUser/Delete',
        data: {
            id: id
        },
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            bootbox.alert(result.Notice);
            $("#user_datatable").dataTable().fnFilter();
        },
        error: function (e) {
            console.log("Delete异常:" + e.responseText);
        }
    });
}