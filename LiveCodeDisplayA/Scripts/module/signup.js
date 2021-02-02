//短信验证码  
var InterValObj; //timer变量，控制时间    
var count = 60; //间隔函数。1秒运行    
var curCount;//当前剩余秒数    
var codeLength = 6;//验证码长度  
var signupIsCommit = false;//避免重复提交
$(".datepicker").datepicker({
    language: "zh-CN",
    autoclose: true,//选中之后自动隐藏日期选择框
    clearBtn: true,//清除按钮
    todayBtn: 'linked',//今日按钮
    format: "yyyy-mm-dd"//日期格式，详见 
});
$('form').bootstrapValidator({
    message: '填写的值无效！',
    feedbackIcons: {
        valid: 'glyphicon glyphicon-ok',
        invalid: 'glyphicon glyphicon-remove',
        validating: 'glyphicon glyphicon-refresh'
    },
    fields: {
        Name: {
            message: '中文姓名验证失败',
            validators: {
                notEmpty: {
                    message: '中文姓名不能为空'
                },
                stringLength: {
                    min: 2,
                    message: '中文姓名长度必须大于2个字符'
                },
            }
        },
        SmsCode: {
            message: '验证码格式错误',
            validators: {
                notEmpty: {
                    message: '验证码不能为空'
                },
                stringLength: {
                    min: 6,
                    max:6,
                    message: '验证码必须大于2个字符'
                },
            }
        },
        Grade: {
            message: '年级验证失败',
            validators: {
                notEmpty: {
                    message: '年级不能为空'
                }
            }
        },
        IDCard: {
            validators: {
                notEmpty: {
                    message: '身份证号码不能为空'
                },
                callback: {
                    message: '身份证号码格式错误',
                    callback: function (value, validator) {
                        if (!value) {
                            return true
                        } else if (isCardNo(value)) {
                            return true;
                        } else {
                            return false;
                        }
                    }
                }
            }
        },
        Phone: {
            validators: {
                notEmpty: {
                    message: '家长联系电话不能为空'
                },
                regexp: {
                    regexp: /^[1][3,4,5,7,8][0-9]{9}$/,
                    message: '请输入正确的手机号',
                    callback: function (value, validator) {
                        alert(111);
                    }
                }              
            }
        }
    }
});
var bootstrapValidator = $('form').data('bootstrapValidator');
// 提交时验证
$('#submitBtn').on('click', function () {
    if (signupIsCommit) {
        alert("请勿重复报名！");
        return;
    }
    bootstrapValidator.validate();
    if (bootstrapValidator.isValid()) {
        $.ajax({
            //几个参数需要注意一下
            type: "POST",//方法类型
            dataType: "json",//预期服务器返回的数据类型
            url: "/Home/SignUpCommit",//url
            data: $('#signUpForm').serialize(),
            success: function (result) {
                if (result.success) {
                    signupIsCommit = true;
                    alert("报名成功！");
                    if (result.JumpToCustomService && result.CustomServiceUrl.length>0) {
                        window.location.replace(result.CustomServiceUrl);
                    }
                }
                else {
                    alert(result.msg)
                }
                curCount = 0;
            },
            error: function () {
                alert("报名失败，请稍后再试！！");
            }
        });
    }
})
// 验证身份证号
function isCardNo(card) {
    let reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
    if (reg.test(card) === false) {
        return false
    } else {
        return true
    }
}
// 验证手机号
function checkMobile(str) {
    let re = /^1\d{10}$/
    if (re.test(str)) {
        return true;
    } else {
        return false;
    }
}

$("#getcode").click(function () {
    var phoneIsValid = $("form").data("bootstrapValidator").isValidField('Phone');
    
    if (!phoneIsValid) {
        $("#telephonenameTip").html("<font color='red'>× 手机号码无效</font>");
        return;
    }
    $("#telephonenameTip").html("");
    //获取输入的手机号码
    var phoNum = $("#Phone").val();
    curCount = count;
    // 向后台发送处理数据    
    $.ajax({
        type: "POST", // 用POST方式传输    
        dataType: "json", // 数据格式:JSON    
        url: "/Home/SendSignUpSms", // 目标地址    
        data: { mobile: phoNum },
        error: function (msg) {
            $("#telephonenameTip").html("<font color='red'>请稍后再尝试发送短信！</font>");
            curCount = 0;
        },
        success: function (data) {
            //前台给出提示语
            if (data.success) {
                // 设置按钮显示效果，倒计时   
                $("#getcode").attr("disabled", "true");
                $("#getcode").val("请在" + curCount + "秒内输入验证码");
                InterValObj = window.setInterval(SetRemainTime, 1000); // 启动计时器。1秒运行一次   
                $("#telephonenameTip").html("<font color='#339933'>√ 已发送(5分钟内有效)</font>");
            } else if (!data.success) {
                $("#telephonenameTip").html("<font color='red'>× 发送失败，" + data.msg + "</font>");
                return false;
            }
        }
    });
});
//timer处理函数    
function SetRemainTime() {
    if (curCount == 0) {
        window.clearInterval(InterValObj);// 停止计时器    
        $("#getcode").removeAttr("disabled");// 启用按钮    
        $("#getcode").val("再次发送验证码");        
    } else {
        curCount--;
        $("#getcode").val("请在" + curCount + "秒后再发送");
    }
}