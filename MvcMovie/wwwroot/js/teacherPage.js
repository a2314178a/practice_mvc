
var myObj = new MyObj();
myObj.queryListUrl = "/Teacher/query";
myObj.downloadPath = "/Teacher/downloadFile";


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

    switch(studentData.sex){
        case "男": copy_txb_row.find("option[value='1']").prop("selected",true);break;
        case "女": copy_txb_row.find("option[value='2']").prop("selected",true);break;
        default: copy_txb_row.find("option[value='0']").prop("selected",true);break;
    }
    
    thisRow.after(copy_txb_row);
    $('.btnActive').css('pointer-events', "none");
}

function updateData(id){
    if(!myObj.isPositiveInteger($(".studentList").find("[name='newSidTxb']").val())){
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
        if(res ==1){
            $('.btnActive').css('pointer-events', "");  
            myObj.reQueryList();
        }else{
            alert('fail');
        }
    };   
    myObj.ajaxFn("post","/Teacher/update",data,successFn);   
}

function delData(stdID, accountID){
    var msg = "您真的確定要刪除嗎？\n\n請確認！";
    if(confirm(msg)==false) //確認視窗
        return;

    var successFn = function(res){
        if(res ==1){
            myObj.reQueryList();
        }
        else{
            alert('fail');
        }     
    };
    myObj.ajaxFn("post","/Teacher/del",{id:stdID, accountID:accountID},successFn);
}

function showDetail(id){

    var successFn = function(res){
        var data = res[0];
        var newTable = $(".template").find(".detailDiv").clone();

        newTable.find("td[name='StudentID']").text(data.studentID);
        newTable.find("td[name='Name']").text(data.name);
        newTable.find("td[name='Sex']").text(myObj.showSex(data.sex));
        var subDate = (data.birth).split("T");
        newTable.find("td[name='Birth']").text(subDate[0]); 
        newTable.find("td[name='Age']").text(data.age);    

        queryStuSelSub(newTable, data.id);

        newTable.find("button[name='editBtn']").attr("onclick","editDetail('"+data.id+"')");
        newTable.find("button[name='updateBtn']").attr("onclick","updateDetail(this,"+ data.id +","+ data.attachID+")");
        newTable.find(".editMode").hide();
        
        $(".listDiv").after(newTable);
        $(".filterDiv,.listDiv").hide();
        $("#mySubjectPage").find("[group='subject']").hide();
    };
    myObj.ajaxFn("get","/Teacher/getStuDetail",{id:id},successFn);
}

function queryStuSelSub(newTable, stuID){
    
    var successFn = function(res){

        var subTimeTable = $(".template").find(".subTable").clone();
        var subGroupTmp = ""; 
        var count = 0;
        res.forEach(function(value){
            var subRow = $(".template").find("tr[name='subTimeDetail']").clone();
            if(subGroupTmp != value.id){
                subTimeTable.find("tr[group='"+subGroupTmp+"']")
                            .find("td[name='subName'],td[name='subStatus']").attr("rowspan",count);
                subRow.attr("group", value.id);
                subGroupTmp = value.id;
                count = 0;
            }else{
                subRow.find("td[name='subName']").remove();
                subRow.find("td[name='subStatus']").remove();
            }
            subRow.find("td[name='subName']").text(value.subjectName);
            myObj.timeSpanToTime(value);
            var dayTime = myObj.weekFormat("getChinese",value.weekDay)+"  "+value.startTime+"~"+value.endTime;
            subRow.find("[name='subTime']").text(dayTime);
            subRow.find("td[name='subStatus']").text(value.status);
            count++;
            subTimeTable.append(subRow);
        });
        subTimeTable.find("tr[group='"+subGroupTmp+"']")
                    .find("td[name='subName'],td[name='subStatus']").attr("rowspan",count);
        newTable.find("td[name='textSubject']").append(subTimeTable);
    }
    myObj.ajaxFn("get", "/Subject/getStuSelSub", {id:stuID}, successFn);
}

function goBackDetail(){
    $(".filterDiv,.listDiv").show();
    $("#mySubjectPage").find("[group='subject']").show();
    $(".mainDiv").find(".detailDiv").remove();
    if($("#studentPage").length > 0){
        myObj.reQueryList();
    }else if($("#mySubjectPage").length > 0){
        showMember(myObj.tabStatus);
    }  
}

function editDetail(stuID){
    var mainTable = $(".mainDiv").find(".detailDiv");

    mainTable.find("input[name='StudentID']").val(mainTable.find("td[name='StudentID']").text());
    mainTable.find("input[name='Name']").val(mainTable.find("td[name='Name']").text());

    switch(mainTable.find("td[name='Sex']").text()){
        case "男": mainTable.find("option[value='1']").prop("selected",true);break;
        case "女": mainTable.find("option[value='2']").prop("selected",true);break;
        default: mainTable.find("option[value='0']").prop("selected",true);break;
    }
    //mainTable.find("input[name='Sex']").val(mainTable.find("td[name='Sex']").text());
    mainTable.find("input[name='Birth']").val(mainTable.find("td[name='Birth']").text());
    mainTable.find("input[name='Age']").val(mainTable.find("td[name='Age']").text());
    mainTable.find(".textMode").hide();
    mainTable.find(".editMode").show();
}

function cancelEditDetail(){
    var mainTable = $(".mainDiv").find(".detailDiv");
    mainTable.find(".textMode").show();
    mainTable.find(".editMode").hide();
}

function updateDetail(thisBtn,id,attachID){
    var thisForm = $(thisBtn).closest("form");
    if(!myObj.isPositiveInteger(thisForm.find("input[name='StudentID']").val()) ||
        !myObj.isPositiveInteger(thisForm.find("input[name='Age']").val())
      )
    {
        alert('學號或年齡欄位輸入有誤');
        return;
    }

    var data = $(".mainDiv .detailDiv form").serialize();
    data = data + "&Id=" + id + "&AttachID=" + attachID + "&SubjectID=";
    
    var successFn = function(res){
        $(".mainDiv").find(".detailDiv").remove();
        showDetail(id);
    }
    myObj.ajaxFn("post","/Teacher/updateDetail",data,successFn)
}


function refreshList(res){
    $(".studentList").empty();
    if(myObj.tabStatus != undefined){
        var selSub = $("#mySubjectSel").val();
        $("#maxPeople").text(myObj.mySubject[selSub]["maxPeople"]);
    }
    if($.isEmptyObject(res)){
        myObj.haveData = false;
        $("#currentPeople").text("0");
        return;
    }
    myObj.haveData = true;
    var count = 0;
    res.forEach(function(value){
        var row = $(".template").find("[name='dataRow']").clone();
        if(myObj.tabStatus != undefined)
        {
            if(value.status == 0){  //待審核
                if(myObj.tabStatus == 0){
                    row.find(".agree_student").attr("onclick","isAgreeMember(1," + value.id +")");    //stuStatus,stuID
                    row.find(".disagree_student").attr("onclick","isAgreeMember(2," + value.id + ")");
                }else{
                    return;
                }
            }else if(value.status == 1){    //接受
                count++;
                if(myObj.tabStatus == 1){
                    row.find(".agree_student").remove();
                    row.find(".disagree_student").attr("onclick","isAgreeMember(2," + value.id + ")").text("踢除");
                }else{
                    return;
                }
            }else{  //status = 2 拒絕
                return;
            }
        }
        
        row.find("[name='sid']").text(value.studentID);
        row.find("[name='name']").find('a').attr("onclick","showDetail("+value.id+");").text(value.name);      
        row.find("[name='sex']").text(myObj.showSex(value.sex));
        row.find(".edit_student").attr("onclick","editData(this," + value.id + ");");
        row.find(".del_student").attr("onclick","delData(" + value.id + "," + value.accountID + ");");
        $(".studentList").append(row);
    });
    $("#currentPeople").text(count);
}



//----------------------------------------------------------------------------




function getMySubject(){
    var successFn = function(res){
        if($.isEmptyObject(res)){
            $("#mySubjectSel").append(new Option("無課程",""));
            return;
        }
        res.forEach(function(value){
            $("#mySubjectSel").append(new Option(value.subjectName,value.id));
            myObj.mySubject[value.id] = {maxPeople : value.maxPeople};
        });
        showMember(1);
    }
    myObj.ajaxFn("get", "/Teacher/getMySubject", null, successFn);
}

function showMember(status=1){
    var subID = $("#mySubjectSel").val();
    if(subID == "")
        return;
    if(status ==1){ //0:待審核  1:已核准 2:課程相關
        $("a[name='isMember']").addClass("active");
        $("a[name='notMember']").removeClass("active");
        $("a[name='subDetail']").removeClass("active");
    }else if(status==0){
        $("a[name='notMember']").addClass("active");
        $("a[name='isMember']").removeClass("active");
        $("a[name='subDetail']").removeClass("active");
    }

    myObj.tabStatus = status;
    var successFn = function(res){
        refreshList(res);
        $("#studentListDiv").show();
        $("#subjectDetailDiv").hide();
        $("#showHomeworkFileDiv").hide();
    }
    myObj.ajaxFn("get", "/Teacher/getSubMember", {subID:subID}, successFn);
}

function isAgreeMember(status, stuID){
    if(status == 2){
        var msg = "您真的確定要拒絕/踢除嗎？\n\n請確認！";
        if(confirm(msg)==false) //確認視窗
            return;
    }else{
        var max = parseInt($("#maxPeople").text());
        var current = parseInt($("#currentPeople").text());
        if(current >= max){
            alert("人數已達上限");
            return;
        }
    }
    
    var subID = $("#mySubjectSel").val();
    var data = {
        Status : status,
        StudentID : stuID,
        SubjectID : subID,
    };
    var successFn = function(res){
        showMember(myObj.tabStatus);
    }
    myObj.ajaxFn("post", "/Teacher/isAgreeMember", data, successFn);
}



function showAddSubTable(){
    var successFn = function(res){
        refreshSubjectList(res);
    };
    myObj.ajaxFn("get", "/Teacher/getMySubjectDetail", null, successFn);
}

function refreshSubjectList(res){
    $(".subjectList").empty();
    if($.isEmptyObject(res)){
        $(".subjectList").append("<tr><td colspan='4' style='text-align:center;'>無資料</td></tr>");
        return;
    }

    var groupTmp = "";
    var count = 0;
    res.forEach(function(value){
        var row = $(".template").find("[name='subjectRow']").clone();
        if(groupTmp != value.id){
            $(".subjectList").find("tr[group='"+groupTmp+"']").find("td[name='subjectName']").attr("rowspan",count);
            row.attr("group", value.id);
            groupTmp = value.id;
            count = 0;
        }else{
            row.find("td[name='subjectName']").remove();
        }
        row.find("[name='subjectName']").text(value.subjectName);
        myObj.timeSpanToTime(value);
        var dayTime = myObj.weekFormat("getChinese",value.weekDay)+"  "+value.startTime+"~"+value.endTime;
        row.find("[name='subjectTime']").text(dayTime);
        row.find(".edit_subject").attr("onclick","editSubject("+value.id+","+value.timeID+");");
        row.find(".del_subject").attr("onclick","delSubject("+value.id+","+value.timeID+");");
        count++;
        $(".subjectList").append(row);
     });
     $(".subjectList").find("tr[group='"+groupTmp+"']").find("td[name='subjectName']").attr("rowspan",count);
}

function closeAddUpdate(){
    $(".add_subject").show();
    $('.btnActive').css('pointer-events', "");
    $("#addSubjectPage").find("[name='addEditDiv']").remove();
    showAddSubTable();
}

function delSubject(subID, timeID){
    var msg = "您真的確定要刪除嗎？\n\n請確認！";
    if(confirm(msg)==false) //確認視窗
        return;

    var successFn = function(res){
        if(res >0){
            showAddSubTable();
        }else{
            alert('fail');
        }     
    };
    myObj.ajaxFn("post","/Teacher/delSubjectTime",{subID:subID, timeID:timeID},successFn);
}

function editSubject(subID, timeID){
    var successFn = function(res){
        var data = res[0];
        myObj.timeSpanToTime(data);
        $(".add_subject").hide();
        $('.btnActive').css('pointer-events', "none");
        
        var addForm = $(".template").find("div[name='addEditDiv']").clone();
        addForm.find("[name='addSubjectBtn'],[name='addSelTime']").remove();
        addForm.find("[name='subjectName']").val(data.subjectName);

        addForm.find("[name='weekDay']").find("option[value='"+data.weekDay+"']").prop("selected",true);
        addForm.find("[name='maxPeople']").val(data.maxPeople);
        addForm.find("[name='subStartTime']").val(data.startTime);
        addForm.find("[name='subEndTime']").val(data.endTime);
        addForm.find("[name='updateSubjectBtn']").attr("onclick", "updateSubject("+subID+","+timeID+");");
        
        $("#addSubjectPage").append(addForm);
        $("html,body").animate({ scrollTop: screen.height }, 500);
    };
    myObj.ajaxFn("get","/Teacher/subDetailByID",{subID:subID, timeID:timeID},successFn);
}

function updateSubject(subID, timeID){
    var SubjectName = $("#addSubjectPage").find("input[name='subjectName']").val();
    var maxPeople = $("#addSubjectPage").find("input[name='maxPeople']").val();
    var WeekDay = $("#addSubjectPage").find("select[name='weekDay']").val();
    var StartTime = $("#addSubjectPage").find("input[name='subStartTime']").val();
    var EndTime = $("#addSubjectPage").find("input[name='subEndTime']").val();
    if(maxPeople=="" || WeekDay=="" || StartTime=="" || EndTime=="" || StartTime >=EndTime){
        alert("欄位皆須選擇或填寫並且格式需正確");
        return;
    }
    var data = {
        Id:subID,
        SubjectName:SubjectName,
        MaxPeople:maxPeople,
        SubjectID:subID,
        SubTimeDetail:[{
            Id:timeID,
            WeekDay:WeekDay,
            StartTime:StartTime,
            EndTime:EndTime
        }], 
    }
    var successFn = function(res){closeAddUpdate();};
    myObj.ajaxFn("post", "/Teacher/updateSubject", data, successFn);
}


//------------------------------------------------------------------------------------------------------


function subjectDetail(){
    $("#studentListDiv").hide();
    $("#subjectDetailDiv").show();
    $("#showHomeworkFileDiv").hide();
    $("a[name='subDetail']").addClass("active");
    $("a[name='notMember']").removeClass("active");
    $("a[name='isMember']").removeClass("active");
    $(".add_homework").show();  
    $('.btnActive').css('pointer-events', ""); 
    var thisSubID = $("#mySubjectSel").val();
    showSubjectDetail(thisSubID);
}

function showSubjectDetail(subID){
    var successFn = function(res){
        $(".homeworkList").empty();
        if(res==null){
            return;
        }
        res.forEach(function(value){
            myObj.sqlTimeProcess(value);
            var newRow = $(".template").find("tr[name='homeworkRow']").clone();
            var link = $("<a href='#'></a>").attr("onclick", "getAllUploadFile("+value.id+")").text(value.homeworkName);
            newRow.find("[name='homeworkName']").append(link);
            newRow.find("[name='homeworkDetail']").text(value.homeworkDetail);
            newRow.find("[name='deadTime']").text(value.uploadDeadTime);
            newRow.find(".edit_homework").attr("onclick","editHomework(this,"+value.id+")");
            newRow.find(".del_homework").attr("onclick","delHomework("+value.id+")");
            $(".homeworkList").append(newRow);
        }); 
    }
    myObj.ajaxFn("get", "/Teacher/getSubHomework", {subID:subID}, successFn);
}


function editHomework(thisBtn, hwID){
    var thisRow = $(thisBtn).closest("tr").addClass('editIng'); ;
    var updateRow = $(".template").find("[name='hwInputRow']").clone(); 
    var htmlTime = myObj.convertToHtmlTime(thisRow.find("[name='deadTime']").text());

    updateRow.find('[group="add"]').remove(); //移除多餘按鈕
    updateRow.find("[name='homeworkName']").val(thisRow.find("[name='homeworkName']").text());
    updateRow.find("[name='homeworkDetail']").val(thisRow.find("[name='homeworkDetail']").text());
    updateRow.find("[name='deadTime']").val(htmlTime);
    updateRow.find("[name='updateBtn']").attr("onclick","updateHomework("+hwID+")");

    $(thisRow).after(updateRow);   
    $('.btnActive').css('pointer-events', "none");  //disable 所有編輯刪除按鈕
}

function delHomework(hwID){
    var msg = "您真的確定要刪除嗎？\n\n請確認！";
    if(confirm(msg)==false) //確認視窗
        return;

    var successFn = function(res){
        if(res >0){
            subjectDetail();
        }else{
            alert('fail');
        }     
    };
    myObj.ajaxFn("post","/Teacher/delHomework",{hwID:hwID},successFn);
}

function updateHomework(hwID){
    var thisRow = $("#newHomeworkDiv").find("tr[name='hwInputRow']");
    var homeworkName = thisRow.find("[name='homeworkName']").val();
    var homeworkDetail = thisRow.find("[name='homeworkDetail']").val();
    var deadTime = thisRow.find("[name='deadTime']").val();
    if(homeworkName=="" || deadTime ==""){
        alert("作業名稱與截止時間皆須填寫");
        return;
    }
    var data = {
        Id: hwID,
        HomeworkName: homeworkName,
        HomeworkDetail: homeworkDetail,
        UploadDeadTime: deadTime
    };
    var successFn = function(res){
        subjectDetail();
    };
    myObj.ajaxFn("post", "/Teacher/updateHomework", data, successFn);
}

function getAllUploadFile(hwID){
    $("#showHomeworkFileDiv").show().find(".homeworkFileList").empty();
    $("#subjectDetailDiv").hide();

    var successFn = function(res){
        res.forEach(function(value){
            var fileRow = $(".template").find("tr[name='homeworkFileRow']").clone();
            fileRow.find("[name='studentID']").text(value.studentID);
            fileRow.find("[name='studentName']").text(value.name);
            var link = $("<a href='#'></a>").attr("onclick", "myObj.downloadFile("+value.id+")").text(value.originalName);
            fileRow.find("[name='file']").append(link);
            fileRow.find("a.del_file").attr("onclick", "delThisFile(this,"+value.id+")");
            $(".homeworkFileList").append(fileRow);
        });
    };
    myObj.ajaxFn("get", "/Teacher/getAllUploadFile", {hwID:hwID}, successFn);
}

function delThisFile(thisBtn, fileID){
    var msg = "您真的確定要刪除嗎？\n\n請確認！";
    if(confirm(msg)==false) //確認視窗
        return;
    var thisRow = $(thisBtn).closest("tr[name='homeworkFileRow']");
    var successFn = function(res){
        if(res > 0){
            thisRow.hide();
        }else{
            alert("del fail");
        }
    }
    myObj.ajaxFn("post", "/Teacher/delFile", {fileID:fileID}, successFn);
}





$(document).ready(function() {   
    
    if($("#studentPage").length > 0){
        myObj.reQueryList();
    }
    else if($("#mySubjectPage").length > 0){
        getMySubject();
    }
    else if($("#addSubjectPage").length >0){
        showAddSubTable();
    }

    //click add Button
    $("[name='addButtonDiv']").on("click", function(){  
        $(this).hide();    //隱藏此按鈕
        if(!myObj.haveData){
            $('.studentList').empty();
        }
        var copy_txb_row = $("[name='inputRow']").clone(); 
        copy_txb_row.find('[group="update"]').remove(); //移除多餘按鈕
        $('.studentList').append(copy_txb_row);    
        $('.btnActive').css('pointer-events', "none");  //disable 所有編輯刪除按鈕
    });

    //click cancel add Button
    $(".studentList,.homeworkList").on("click", "[name='addCancel']", function(){                      
        $("[name='addButtonDiv'],.add_homework").show();  //顯示新增學生按鈕
        $('.btnActive').css('pointer-events', "");  //enable 編輯刪除按鈕 
        $(this).closest("tr").remove();     //刪除此列
        /*if(!myObj.haveData){
            $('.studentList').append("<tr><td colspan='4' style='text-align:center;'>無資料</td></tr>");
        }*/
    });
   
    //click add data Button
    $(".studentList").on("click","[name='addBtn']", function(){
        var thisRow = $(this).closest("tr");    
        if(!myObj.isPositiveInteger(thisRow.find("[name='newSidTxb']").val())){
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
                myObj.reQueryList();
            }
            else{
                alert('fail');
            }    
        };
        myObj.ajaxFn("post","/Teacher/create",data,successFn);
    });//.on(click,function)
    
    $('.studentList,.homeworkList').on('click', "[name='updateCancel']", function(){
        var thisRow = $(this).closest('tr');
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
        if(selOption=="sex")
            inputWord = $('[name="filterSex"]').val();

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
            myObj.ajaxFn("get","/Teacher/queryFilter",data,successFn);
        }
    });

    $("form [name='filterOption']").on("change", function(){ 
        if($(this).val() == "sex"){
            $('[name="filterWord"]').hide();
            $('[name="filterSex"]').show();
        }else{
            $('[name="filterWord"]').show();
            $('[name="filterSex"]').hide();
        }
    });//.on(submit,function)

    $("#mySubjectSel").on("change", function(){ 
        var active = $("#mySubjectPage").find("a[name='subDetail']").hasClass("active");
        if(!active){
            showMember(myObj.tabStatus);
        }
        else{
            subjectDetail();
        }
    });//.on(submit,function)

    $(".add_subject").on("click",function(){
        $(this).hide();
        $('.btnActive').css('pointer-events', "none");
        var addForm = $(".template").find("div[name='addEditDiv']").clone();
        addForm.find("[name='updateSubjectBtn']").remove();
        $("#addSubjectPage").append(addForm);
    });

    $("#addSubjectPage").on("click", "[name='addSelTime']", function(){
        var thisDiv = $("#addSubjectPage").find("[name='addEditDiv']");
        var selDay =  thisDiv.find("select[name='weekDay']").val();
        var subStartTime = thisDiv.find("input[name='subStartTime']").val();
        var subEndTime = thisDiv.find("input[name='subEndTime']").val();

        if(selDay == "" || subStartTime == "" || subEndTime == "" || subStartTime >= subEndTime){
            alert("星期跟時間都需選擇 或者格式有誤");
            return;
        }
        selDay = myObj.weekFormat("getChinese", selDay);   
        var delBtn = "<button type='button' onclick='delDayTime(this)' class='delDayTimeBtn'>X</button>";
        var day_time = "<p name='subjectDayTime' >"+ selDay + "  " + subStartTime + "~" + subEndTime + delBtn + "</p>";
        thisDiv.find("[name='showDiv']").append(day_time);
    });

    $("#addSubjectPage").on("click", ".delDayTimeBtn", function(){
        $(this).closest("P").remove();
    });

    $("#addSubjectPage").on("click", "[name='addSubjectBtn']", function(){
        myObj.dataCheck("add", "subject");
        if(myObj.errorCode>0){
            myObj.callAlert(myObj.errorCode);
            return;
        }
        var SubjectName = $("#addSubjectPage").find("input[name='subjectName']").val();
        var maxPeople = $("#addSubjectPage").find("input[name='maxPeople']").val();
        var getSubjectTime = $("#addSubjectPage").find("p[name='subjectDayTime']");
        var successFn = function(res){};
        var data = {
            SubjectName : SubjectName,
            MaxPeople : maxPeople,
            SubTimeDetail : [],
        }
        var count = 0;
        getSubjectTime.each(function(){
            var str = $(this).prop('firstChild').nodeValue; //取得第一個文字節點的值
            var strArr = str.split("  ");
            var WeekDay = myObj.weekFormat("getValue", strArr[0]);   
            var StartTime = strArr[1].split("~")[0];
            var EndTime = strArr[1].split("~")[1];
            data.SubTimeDetail[count] = {
                WeekDay : WeekDay,
                StartTime : StartTime,
                EndTime : EndTime
            }
            count++;
        });
        myObj.ajaxFn("post", "/Teacher/createSubject", data, successFn) ;  
        var timer = setInterval(function() {  
                closeAddUpdate();
                clearInterval(timer); 
            } 
        , 100);  
    });

    $("#subjectDetailDiv").on("click", ".add_homework", function(){  
        $(this).hide();    //隱藏此按鈕
        var addRow = $(".template").find("[name='hwInputRow']").clone(); 
        addRow.find('[group="update"]').remove(); //移除多餘按鈕
        $('.homeworkList').append(addRow);    
        $('.btnActive').css('pointer-events', "none");  //disable 所有編輯刪除按鈕
    });


    $(".homeworkList").on("click", "[name='addBtn']", function(){
        var thisRow = $(this).closest("tr");    
        var homeworkName = thisRow.find("[name='homeworkName']").val();
        var deadTime = thisRow.find("[name='deadTime']").val();
        if(homeworkName=="" || deadTime==""){
            alert("作業名稱與截止時間皆須填寫");
            return;
        }

        var data = {
            SubjectID: $("#mySubjectSel").val(),
            SubjectName: $("#mySubjectSel option:selected").text(), 
            HomeworkName: homeworkName,
            HomeworkDetail: thisRow.find("[name='homeworkDetail']").val(),
            UploadDeadTime: deadTime
        };
        
        var successFn = function(res){
            if(res >0){
                subjectDetail();
            }else{
                alert('fail');
            } 
        };
        myObj.ajaxFn("post", "/Teacher/homeworkCreate", data, successFn);
    });//.on(click,function)










});//.ready function





    

   