
class MyObj{
    
    constructor() {
        this.loginID = null;   
        this.targetID = null;
        this.haveData = false;
        this.subWin = null;
        this.errorCode = 0;
        this.queryType = "get";
        this.queryListUrl = "";
        this.querySubListUrl="";
        this.mySubject = {};
        this.downloadPath = "";
    }

    callAlert(code){
        switch(code){
            case 1: alert("欄位都須填寫或選擇");break;
            case 2: alert("密碼並不一致，請重新輸入");break;
            case 3: alert("請輸入科目名稱");break;
            case 4: alert("請選擇授課老師");break;
            case 5: alert("請至少添加一筆授課時間");break;
            case 6: alert("請輸入人數上限");break;
        }        
    }
    ajaxFn(type,url,data,successFn){
        $.ajax({
            type : type,
            url : url,     
            data: data,       
            success:function(res){
                if(res == -2){
                    alert("此帳號已有其他使用者登入，請重新登入");
                    if(window.name == "subWindow"){
                        window.close();
                        window.opener.location.href = "/Account/logOut";
                    }else{
                        window.location.href = "/Account/logOut";
                    }
                }else if(res==1062){
                    alert("已有相關資料 請勿重複");
                }else{
                    successFn(res);
                }
            },
            error:function(){alert("error");}
        });   
    }
    uploadAjax(type,url,data,successFn){
        $.ajax({
            type : type,
            url : url,     
            data: data,
            processData: false,  // 禁止將data自動轉換成query string
            contentType: false, // 取消預設值'application/x-www-form-urlencoded; charset=UTF-8'       
            success:function(res){
                if(res == -2){
                    alert("此帳號已有其他使用者登入，請重新登入");
                    if(window.name == "subWindow"){
                        window.close();
                        window.opener.location.href = "/Account/logOut";
                    }else{
                        window.location.href = "/Account/logOut";
                    }
                }else if(res==1062){
                    alert("已有相關資料 請勿重複");
                }else{
                    successFn(res);
                }
            },
            error:function(){alert("error");}
        });   
    }
    paddingLeft(str,length){
        if(str.length >= length)
            return str;
        else
            return this.paddingLeft("0" +str,length);
    }
    timeSpanToTime(value){
        var s_hours = value.startTime.hours.toString(); 
        s_hours = this.paddingLeft(s_hours,2);
        var s_minutes = value.startTime.minutes.toString(); 
        s_minutes = this.paddingLeft(s_minutes,2);
        var e_hours = value.endTime.hours.toString(); 
        e_hours = this.paddingLeft(e_hours,2);
        var e_minutes = value.endTime.minutes.toString(); 
        e_minutes = this.paddingLeft(e_minutes,2);
        value.startTime = s_hours + ":" + s_minutes;
        value.endTime = e_hours + ":" + e_minutes;
    }
    sqlTimeProcess(value){
        value.uploadDeadTime = (value.uploadDeadTime).replace("T", " ");
    }
    convertToHtmlTime(time){
        return time.replace(" ", "T");
    }
    openSubWindow(url){
        $('.btnActive').css('pointer-events', "none");
        var win_width = 500;
        var win_height = 500;
        var PosX = (parseInt(screen.width-win_width))/2; 
        var PosY = (parseInt(screen.height-win_height))/2; 
        var parameter = "height=" + win_height + ",width=" + win_width + ",top=" + PosY + ",left=" + PosX +
                ",toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no,status=no";       
        this.subWin = window.open(url, "subWindow", parameter);
        this.lookSubWin();
    }
    lookSubWin(){
        var timer = setInterval(function(){  
            if(myObj.subWin.closed){
                $('.btnActive').css('pointer-events', "");
                myObj.reQueryList();
                clearInterval(timer); 
            } 
        }, 1000);
    }
    reQueryList(requestType=this.queryType, url=this.queryListUrl, data=null, successFn=null){
        if(successFn==null){
            var successFn = function(res){
                refreshList(res);
            }
        }
        this.ajaxFn(requestType, url, data, successFn);
    }
    QuerySubjectList(requestType=this.queryType, url=this.querySubListUrl, data=null, successFn=null){
        var successFn = function(res){
            refreshSubjectList(res);
        }
        this.ajaxFn(requestType, url, data, successFn);
    }
    isPositiveInteger(s){
        var re = /^[0-9]+$/ ;
        return re.test(s);
    }
    dataCheck(action, type){
        this.errorCode = 0;  
    
        if(type == "user"){
            if($("[name='Password']").val() != $("[data='rePassword']").val()){
                this.errorCode = 2;
            }
            $("[group='detail']").each(function(){
                if($(this).prop("type") =="password" && action =="update")return;
                if($(this).val()==""){
                    myObj.errorCode = 1;
                    return false;
                }
            }); 
        }
        else if(type == "subject"){
            var SubjectName = $("#addSubjectPage").find("input[name='subjectName']").val();
            if(SubjectName == ""){
                this.errorCode = 3;
            }
            var SubjectTeacher = $("#addSubjectPage").find("select[name='selTeacher']").val();
            if(SubjectTeacher == ""){
                this.errorCode = 4;
            }
            var getSubjectTime = $("#addSubjectPage").find("p[name='subjectDayTime']");
            if(getSubjectTime.length ==0){
                this.errorCode = 5;
            }
            var subMaxPeople = $("#addSubjectPage").find("input[name='maxPeople']").val();
            if(subMaxPeople ==""){
                this.errorCode = 6;
            }
        }
    }
    weekFormat(type,value){
        if(type=="getValue"){
            var week = {
                星期一:"1", 星期二:"2", 星期三:"3", 星期四:"4",  
                星期五:"5", 星期六:"6", 星期日:"7",
            };
            return week[value];
        }
        else if(type=="getChinese"){
            var week = {
                1:"星期一", 2:"星期二", 3:"星期三", 4:"星期四",
                5:"星期五", 6:"星期六", 7:"星期日",
            };
            return week[value];
        }
    }
    showSex(sexCode){
        switch(sexCode){
            case 1:return "男";
            case 2:return "女";
            default:return "不公開";
        }
    }
    downloadFile(hwUrlID){
        var successFn = function(res){
            var a = document.createElement('a');
            var rootPath = myObj.getRootPath();
            //a.href = encodeURI(res.abFileURL);
            a.href = rootPath + "/" + res.abFileURL;
            a.download = res.originalName;
            a.click(); 
        }
        this.ajaxFn("post", this.downloadPath, {hwUrlID: hwUrlID}, successFn);
    }
    getRootPath(){
        var strFullPath = window.document.location.href;
        var strPath = window.document.location.pathname;
        var pos = strFullPath.indexOf(strPath);
        var prePath = strFullPath.substring(0,pos);
        return(prePath);
    }
    chkExtName(file){
        var fileTypes = ['pptx', 'xlsx', 'docx', 'pdf'];
        if(file){
            var fileExt = file.name;
            fileExt = fileExt.substring(fileExt.lastIndexOf('.')+1);
        }
        if(!file || fileTypes.indexOf(fileExt) < 0){
            return false;
        }   
        return true;
    }



}//class








    

   