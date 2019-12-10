

function editData(thisBtn,id){
    var thisRow = $(thisBtn).closest('tr').addClass('editIng'); 
    var studentData={
        sid:thisRow.find("[name='sid']").text(),
        name:thisRow.find("[name='name']").text(),
        sex:thisRow.find("[name='sex']").text()
    };

    var copy_txb_row = $("[name='inputRow']").clone();
    copy_txb_row.find('[group="add"]').remove();
    copy_txb_row.find("[name='updateBtn']").attr("onclick","updateData("+id+")");
    copy_txb_row.find("[name='newSidTxb']").val(studentData.sid);
    copy_txb_row.find("[name='newNameTxb']").val(studentData.name);
    copy_txb_row.find("[name='newSexTxb']").val(studentData.sex);
    thisRow.after(copy_txb_row);
    $('.btnActive').css('pointer-events', "none");
}

function updateData(id){
    if(!isPositiveInteger($(".studentList").find("[name='newSidTxb']").val())){
        alert('學號欄位輸入有誤');
        return;
    }
    var data = {
        Id:id,
        StudentID : $(".studentList").find("[name='newSidTxb']").val(),
        Name : $(".studentList").find("[name='newNameTxb']").val(),
        Sex : $(".studentList").find("[name='newSexTxb']").val()
    }; 
    var successFn=function(res){
        if(res >0){
            $('.btnActive').css('pointer-events', "");  
            reQueryList();
        }
        else{
            alert('fail');
        }
    };
    
    ajaxFn("post","CURD/update",data,successFn);
}

function delData(id){
    var msg = "您真的確定要刪除嗎？\n\n請確認！";
    if(confirm(msg)==false) //確認視窗
        return;

    var successFn = function(res){
        if(res >0){
            reQueryList();
        }
        else{
            alert('fail');
        }     
    };
    ajaxFn("post","CURD/del",{id:id},successFn);
}

function reQueryList(){
    var successFn = function(res){
        refreshList(res);
    }
    ajaxFn("get","CURD/query",null,successFn);
}

function refreshList(res){
    $(".studentList").empty();
    if($.isEmptyObject(res)){
        $(".studentList").append("<tr><td colspan='4' style='text-align:center;'>無資料</td></tr>");
        pageParameter.haveData = false;
        return;
    }
    pageParameter.haveData = true;
    res.forEach(function(value){
       var row = $(".template").find("[name='dataRow']").clone();
       row.find("[name='sid']").text(value.studentID);
       row.find("[name='name']").text(value.name);
       row.find("[name='sex']").text(value.sex);
       row.find(".edit_student").attr("onclick","editData(this,"+value.id+");");
       row.find(".del_student").attr("onclick","delData("+value.id+");");
       $(".studentList").append(row);
    });
}

function isPositiveInteger(s){//是否為正整數
    var re = /^[0-9]+$/ ;
    return re.test(s)
}



function ajaxFn(type,url,data,successFn){
    $.ajax({
        type : type,
        url : url,     
        data: data,       
        success:function(res){
            successFn(res);
        },
        error:function(){alert("error");}
    });   
}

var pageParameter = {
    haveData: false,
}

$(document).ready(function() {
          
    reQueryList();   
    
    //click add Button
    $("[name='addButtonDiv']").on("click", function(){  
        $(this).hide();    //隱藏此按鈕
        if(!pageParameter.haveData){
            $('.studentList').empty();
        }
        var copy_txb_row = $("[name='inputRow']").clone(); 
        copy_txb_row.find('[group="update"]').remove(); //移除多餘按鈕
        $('.studentList').append(copy_txb_row);    
        $('.btnActive').css('pointer-events', "none");  //disable 所有編輯刪除按鈕
    });

    //click cancel add Button
    $(".studentList").on("click", "[name='addCancel']", function(){                      
        $("[name='addButtonDiv']").show();  //顯示新增學生按鈕
        $('.btnActive').css('pointer-events', "");  //enable 編輯刪除按鈕 
        $(this).closest("tr").remove();     //刪除此列
        if(!pageParameter.haveData){
            $('.studentList').append("<tr><td colspan='4' style='text-align:center;'>無資料</td></tr>");
        }
    });
   
    //click add data Button
    $(".studentList").on("click","[name='addBtn']", function(){
        var thisRow = $(this).closest("tr");    
        if(!isPositiveInteger(thisRow.find("[name='newSidTxb']").val())){
            alert('學號欄位輸入有誤');
            return;
        }

        var data = {
            StudentID : thisRow.find("[name='newSidTxb']").val(),
            Name : thisRow.find("[name='newNameTxb']").val(),
            Sex : thisRow.find("[name='newSexTxb']").val()
        }; 
        
        var successFn = function(res){
            if(res >0){
                $("[name='addButtonDiv']").show(); 
                $(".btnActive").css('pointer-events', "");
                reQueryList();
            }
            else{
                alert('fail');
            }    
        };

        ajaxFn("post","CURD/create",data,successFn);
    });//.on(click,function)
    
    $('.studentList').on('click', "[name='updateCancel']", function(){
        var thisRow = $(this).closest('tr');
        //var tmp = thisRow.closest('exp'); //exp 只能是元素
        //var tmp = thisRow.prev('exp');只找上一個兄弟元素,exp可過濾，最後結果只有1個或0個
        //var tmp = thisRow.prevAll('exp');往上找所有兄弟元素,exp可過濾
        thisRow.prevAll('tr.editIng').removeClass('editIng');
        thisRow.remove();
        $('.btnActive').css('pointer-events', "");
    });



    $("#filter").on("submit", function(e){ 
        e.preventDefault();
        var selOption = $('[name="filterOption"]').val();
        var inputWord = $('[name="filterWord"]').val();
        if(inputWord==""){
            reQueryList();
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
            ajaxFn("get","CURD/query_option",data,successFn);
        }
    });





});//.ready function


function ajaxErrorFunction(jqXHR, textStatus, errorThrown)
{
    alert("當前狀態: " +jqXHR.readyState);       
    alert("HTTP 狀態碼: "+jqXHR.status);
    alert("狀態碼對應訊息: " +jqXHR.statusText);
    alert("服務端返回訊息: " +jqXHR.responseText);
    alert("錯誤類型textStatus: "+textStatus);
    alert("異常對象errorThrown: "+errorThrown); 
}
   


    

   