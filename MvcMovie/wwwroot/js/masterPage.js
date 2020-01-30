
var myObj = new MyObj();
myObj.queryListUrl = "/Account/query";
myObj.querySubListUrl = "/Subject/query";




function updateData(id){
    myObj.dataCheck("update","user");
    if(myObj.errorCode>0){
        myObj.callAlert(myObj.errorCode);
        return;
    }
    var data = $("form").serialize();
    data = data +"&Id="+id;
    var successFn=function(res){
        if(res== 1){
            window.close();
        }else if(res==0){
            alert("fail or not update because all value is not change");
        }else if(res==-1){
            alert("該帳號已存在");
        }
    };   
    myObj.ajaxFn("post","/Account/update",data,successFn);
}

function editData(thisBtn,id){
    myObj.openSubWindow("/Account/updateAccountForm?id="+id);
}

function refreshList(res){
    $(".teacherList").empty();
    if($.isEmptyObject(res)){
        $(".teacherList").append("<tr><td colspan='4' style='text-align:center;'>無資料</td></tr>");
        myObj.haveData = false;
        return;
    }
    myObj.haveData = true;

    res.forEach(function(value){
       var row = $(".template").find("[name='dataRow']").clone();
       row.find("[name='account']").text(value.account);
       row.find("[name='userName']").text(value.userName);
       var userType = function(){
            switch(value.authority){
                case 1: return "管理人員";
                case 2: return "教師";
                case 3: return "學生";
                default : return "";
            }
       }
       row.find("[name='authority']").text(userType);
       row.find(".edit_user").attr("onclick","editData(this,"+value.id+");");
       row.find(".del_user").attr("onclick","delData("+value.id+");");
       $(".teacherList").append(row);
    });
}

function delData(id){
    var msg = "您真的確定要刪除嗎？\n\n請確認！";
    if(confirm(msg)==false) //確認視窗
        return;

    var successFn = function(res){
        if(res>0){
            myObj.reQueryList();
        }else{
            alert('fail');
        }     
    };
    myObj.ajaxFn("post","/Account/del",{id:id},successFn);
}




$(document).ready(function() {
    $('[name="filterType"]').hide();

    if($("#accountPage").length > 0){
        myObj.reQueryList();
    }

    $("[name='addButtonDiv']").on("click", function(){  
        myObj.openSubWindow("/Account/addAccountForm");
    });

    $("[name='cancelEdit'").on("click",function(){
        window.close(); 
    });

    $("[name='addUser'").on("click",function(){
        myObj.dataCheck("add","user");
        if(myObj.errorCode>0){
            myObj.callAlert(myObj.errorCode);
            return;
        }

        var data = $("form").serialize();
        var successFn = function(res){
            if(res== 1){
                window.close();
            }else if(res==0){
                alert("fail");
            }else if(res==-1){
                alert("該帳號已存在");
            }
        };
        myObj.ajaxFn("post","/Account/create",data,successFn);
    });

    $("#filter").on("submit", function(e){ 
        e.preventDefault();
        var selOption = $('[name="filterOption"]').val();
        var inputWord = $('[name="filterWord"]').val();
        if(selOption=="authority")
            inputWord = $('[name="filterType"]').val();

        if(inputWord==""){
            myObj.reQueryList();
        }
        else if(selOption==0 ){
            alert("請選擇查詢欄位");
        }
        else{
            var data={
                filterOption: selOption,
                filterWord: inputWord
            };
            var successFn = function(res){
                refreshList(res);
            };
            myObj.ajaxFn("get","/Account/query_filter",data,successFn);
        }
    });

    $("form [name='filterOption']").on("change", function(){ 
        if($(this).val() == "authority"){
            $('[name="filterWord"]').hide();
            $('[name="filterType"]').show();
        }else{
            $('[name="filterWord"]').show();
            $('[name="filterType"]').hide();
        }
    });//.on(submit,function)


    $(window).bind('beforeunload',function(){
        if(myObj.subWin != null && myObj.subWin.open){
            myObj.subWin.close();
        }
    });
});//.ready function



   


    

   