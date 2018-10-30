
//绑定平台
function bindPlatformsCombox(id) {

    $.ajax({
        type: 'GET',
        url: '/Platform/GetPlatformsForCombox',
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

//绑定应用
function bindAppsForCombox(id) {

    $.ajax({
        type: 'GET',
        url: '/App/GetAppsForCombox',
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

function GetAppKey() {

    $.ajax({
        url: '/DeveloperApply/BuildAppKey',
        data: null,
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            if (result.Bresult) {
                $('#AppKey2').val(result.Notice);
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

function GetAppSecrect() {

    $.ajax({
        url: '/DeveloperApply/BuildAppSecrect',
        data: null,
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            if (result.Bresult) {
                $('#AppSecrect2').val(result.Notice);
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

function ShowModal(id, appId, platformId, appKey, appSecrect) {
    if (id == 0) {
        //add
        $('#developApplyModal .modal-title').html('添加');
        $('#Id').val('');
        $('#dp_platform2').val('');
        $('#dp_app2').val('');
        $('#AppKey2').val('');
        $('#AppSecrect2').val('');

        $('#developApplyModal').modal('show');
    } else {
        //update
        $('#developApplyModal .modal-title').html('修改');

        $('#Id').val(id);
        $('#dp_platform2').val(platformId);
        $('#dp_app2').val(appId);
        $('#AppKey2').val(appKey);
        $('#AppSecrect2').val(appSecrect);

        $('#developApplyModal').modal('show');
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
    var platformId = $('#dp_platform2').val();
    var appId = $('#dp_app2').val();
    var appKey = $('#AppKey2').val();
    var appSecrect = $('#AppSecrect2').val();

    //1.validation
    if (platformId == '' || platformId == null || platformId == undefined || platformId==0) {
        bootbox.alert("平台必选！");
        return;
    }

    if (appId == '' || appId == null || appId == undefined || appId==0) {
        bootbox.alert("应用必选！");
        return;
    }

    if (appKey == '' || appKey==undefined || appKey == null) {
        bootbox.alert("AppKey必填!");
        return;
    }

    if (appSecrect == '' || appSecrect==undefined || appSecrect == null) {
        bootbox.alert("AppKey必选!");
        return;
    }

    //2.add
    $.ajax({
        url: '/DeveloperApply/Create',
        data: {
            platformId: platformId,
            appId: appId,
            appKey: appKey,
            appSecrect: appSecrect
        },
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (result) {
            if (result.Bresult) {
                $('#developApplyModal').modal('hide');
                $("#develop_apply_datatable").dataTable().fnFilter();
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