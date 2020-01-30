
var myObj = new MyObj();
myObj.downloadPath = "/Student/downloadFile";


function reQueryDetail(){
    var successFn = function(res){
        refreshDetail(res);
    }
    myObj.ajaxFn("get","/Student/query",null,successFn);
}

function refreshDetail(res){
    var data = res[0];
    $("#studentDetail").find("td[name='StudentID']").text(data.studentID);
    $("#studentDetail").find("td[name='Name']").text(data.name);
    $("#studentDetail").find("td[name='Sex']").text(myObj.showSex(data.sex));
    var subDate = (data.birth).split("T");
    $("#studentDetail").find("td[name='Birth']").text(subDate[0]);
    $("#studentDetail").find("td[name='Age']").text(data.age);
    $("#editDiv").find("button[name='addDelBtn']").attr("onclick", "showSubDiv("+ data.id +")");
    $("#editDiv").find("button[name='addSubBtn']").attr("onclick", "addSubject("+ data.id +")");
    
    queryMySubDetail("show", data.id);
}


function queryMySubDetail(type="", stuID){
    var successFn = function(res){
        $("#studentDetail").find("td[name='Subject']").empty();
        var subTimeTable = $(".template").find(".subTable").clone();
        var subGroupTmp = "";
        var count = 0;
        res.forEach(function(value){
            var subRow = $(".template").find("tr[name='subTimeDetail']").clone();
            if(subGroupTmp != value.id){
                subTimeTable.find("tr[group='"+subGroupTmp+"']")
                            .find("td[name='subName'],td[name='subStatus'],td[name='delBtn']").attr("rowspan",count);
                subRow.attr("group", value.id);
                subGroupTmp = value.id;
                count = 0;
            }else{
                subRow.find("td[name='subName']").remove();
                subRow.find("td[name='subStatus']").remove();
                subRow.find("td[name='delBtn']").remove();
            }
            subRow.find("td[name='subName']").text(value.subjectName);
            myObj.timeSpanToTime(value);
            var dayTime = myObj.weekFormat("getChinese",value.weekDay)+"  "+value.startTime+"~"+value.endTime;
            subRow.find("[name='subTime']").text(dayTime);
            subRow.find("td[name='subStatus']").text(value.status);
            if(type=="update"){
                var delSubBtn =$("<input type='button' value='x' class='delDayTimeBtn'>");
                delSubBtn.attr("onclick", "delThisSub(this,"+ stuID + ",'"+subGroupTmp+"')");   //subGroupTmp = subID
                subRow.find("td[name='delBtn']").html(delSubBtn);
            }
            count++;
            subTimeTable.append(subRow);

        });
        subTimeTable.find("tr[group='"+subGroupTmp+"']")
                    .find("td[name='subName'],td[name='subStatus'],td[name='delBtn']").attr("rowspan",count);
        $("#studentDetail").find("td[name='Subject']").append(subTimeTable);
    }
    myObj.ajaxFn("get", "/Subject/getStuSelSub", {id:stuID}, successFn);
}




function showSubDiv(studentID){
    $("#selSubjectDiv").show();
    $("#editDiv").find("button[name='addDelBtn']").hide();

    queryMySubDetail("update", studentID);

    var successFn = function(res){
        var selectList = $("#selSubjectDiv").find("select").empty();
        var txt = "請選擇";
        var subGroupTmp = "";
        res.forEach(function(value){
            myObj.timeSpanToTime(value);
            var day = myObj.weekFormat("getChinese", value.weekDay);

            if(subGroupTmp == value.id){
                txt = txt + "  " + day +"  " +value.startTime +":"+ value.endTime;
            }
            else{
                selectList.append(new Option(txt, subGroupTmp));    //subGroupTmp = subjectID
                txt = value.subjectName + " - " + day +"  " +value.startTime +":"+ value.endTime;
                subGroupTmp = value.id;
            }
        });
        selectList.append(new Option((txt), subGroupTmp));     
    }
    myObj.ajaxFn("get", "/Subject/getAllSubject", null, successFn);
}

function delThisSub(thisBtn, delStuID, groupTag){
    var context = $(thisBtn).closest("tr").text();
    var msg = "您真的確定要退選  "+context+"\n請確認！";
    if(confirm(msg)==false) //確認視窗
        return;
    var  successFn = function(res){
        if(res>0){
            showSubDiv(delStuID);
        }else{
            alert("fail");
        }
    }
    myObj.ajaxFn("post", "/Student/stuDelSub", {studentID:delStuID, subjectID:groupTag}, successFn);
}

function cancelAddDel(){
    $("#editDiv").find("button[name='addDelBtn']").show();
    $("#selSubjectDiv").find("select").empty();
    $("#selSubjectDiv").hide();
    reQueryDetail();
}

function addSubject(stuID){
    var subID = $("#subjectSel").val();
    if(subID == ""){
        alert("請選擇課程");return;
    }
    var successFn = function(res){
        if(res==1){
            showSubDiv(stuID);
        }else if(res==-1){
            alert("此課程時間與已選課程時間有所衝突");
        }
        else{
            alert("fail");
        }
    }
    myObj.ajaxFn("post", "/Student/addSelSub",  {studentID:stuID, subjectID:subID}, successFn);
}


function getMySubjectTable(){
    var successFn = function(res){
        res.forEach(function(value){
            myObj.timeSpanToTime(value);
            var timeFirst = parseInt((value.startTime).split(":")[0]);
            var timeSecond = parseInt((value.endTime).split(":")[0]);
            var step = timeSecond - timeFirst;
            for(step; step >0; step--){
                var idStr = value.weekDay + "_" + timeFirst + "_" + (++timeFirst);
                var link = $("<a href='#'></a>").attr("onclick", "showSubDetail("+ value.id+")").text(value.subjectName);
                $("#"+ idStr).append(link);
            }
        });
    };
    myObj.ajaxFn("get","/Student/getVerifySubject",null,successFn);
}


function showSubDetail(subID){
    myObj.homeworkSubID = subID;
    hideUploadDiv();
    $("#mySubjectTime").hide();
    $(".homeworkList").empty();
    var successFn = function(res){
        res.forEach(function(value){
            myObj.sqlTimeProcess(value);
            var newRow = $(".template").find("tr[name='homeworkRow']").clone();
            newRow.find("[name='homeworkName']").text(value.homeworkName);
            newRow.find("[name='homeworkDetail']").text(value.homeworkDetail);
            newRow.find("[name='deadTime']").text(value.uploadDeadTime);
            if(value.urlID>0){
                var link = $("<a href='#'></a>").attr("onclick","myObj.downloadFile("+value.urlID+")").text(value.originalName);
                newRow.find("[name='fileView']").append(link);
            }
            newRow.find(".upload_homework").attr("onclick", "showUploadDiv(this,'"+value.homeworkName+"',"+value.id+")");
            $(".homeworkList").append(newRow);
        });
        $("#thisSubHomework").show();
    };
    myObj.ajaxFn("get","/Student/getThisSubHomework",{subID: subID},successFn);
}


function showUploadDiv(thisBtn, hwName, hwID){
    hideUploadDiv();
    var uploadDiv = $("#uploadDiv");
    var thisRow = $(thisBtn).closest("tr[name='homeworkRow']");
    var deadTime = $(thisRow).find("[name='deadTime']").text();
    var nowTime = new Date();
    if((Date.parse(nowTime)).valueOf() > (Date.parse(deadTime)).valueOf()){
        alert("已過截止時間");
        return;
    }


    if(thisRow.find("td[name='fileView']").find("a").length >0){
        var msg = "該作業已繳交，是否要重新上傳？(舊的作業將被刪除) \n請確認！";
        if(confirm(msg)==false) //確認視窗
            return;
    } 
    uploadDiv.find("h5").text(hwName+" 檔案上傳");
    uploadDiv.find("#uploadSubmit").attr("onclick", "uploadFileFn("+hwID+")");
    uploadDiv.show();
}

function hideUploadDiv(){
    var uploadDiv = $("#uploadDiv").hide();
    uploadDiv.find("#uploadSubmit").removeAttr("onclick");
    $("#uploadFile").val("");
}

function uploadFileFn(hwID){
    var file = $("#uploadFile").get(0).files[0];
    if(!myObj.chkExtName(file)){
        alert("無偵測到檔案或不支援此檔案類型");
        return;
    }
    var formData = new FormData();
    formData.append("StudentFile", file);
    formData.append("SubHomeworkID", hwID);

    var upFileSuccessFn = function(res){
        if(res == 1){
            showSubDetail(myObj.homeworkSubID);
        }else{
            alert("繳交失敗")
        }
    };
    myObj.uploadAjax("post", "/Student/uploadFile", formData , upFileSuccessFn);
}









$(document).ready(function() {
    if($("#stuDetailPage").length >0){   
        reQueryDetail();
    }
    else if($("#stuMySubjectPage").length >0){
        getMySubjectTable();
    }

});//.ready function

















    

   