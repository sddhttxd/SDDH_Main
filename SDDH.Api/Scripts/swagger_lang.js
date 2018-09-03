///    <summary>
/// 中文转换1
///    </summary>

var SwaggerTranslator = (function () {
    //定时执行检测是否转换成中文,最多执行500次  即500*50/1000=25s
    var iexcute = 0,
    //中文语言包
    _words = {
        "Warning: Deprecated": "警告：已过时",
        "Implementation Notes": "实现备注", 
        "Response Class": "响应类",
        "Status": "状态",
        "Parameters": "参数",
        "Parameter": "参数",
        "Value": "值",
        "Description": "描述",
        "Parameter Type": "参数类型",
        "Data Type": "数据类型",
        "Response Messages": "响应消息",
        "HTTP Status Code": "HTTP状态码",
        "Reason": "原因",
        "Response Model": "响应模型",
        "Request URL": "请求URL",
        "Response Body": "响应体",
        "Response Code": "响应码",
        "Response Headers": "响应头",
        "Hide Response": "隐藏响应",
        "Headers": "头",
        "Try it out!": "试一下！",
        "Show/Hide": "展开/收起",
        "List Operations": "显示列表",
        "Expand Operations": "展开所有",
        "Raw": "原始",
        "can't parse JSON.  Raw result": "无法解析JSON. 原始结果",
        "Model Schema": "模型架构",
        "Model": "模型",
        "Example Value": "示例",
        "apply": "应用",
        "Username": "用户名",
        "Password": "密码",
        "Terms of service": "服务条款",
        "Created by": "创建者",
        "See more at": "查看更多：",
        "Contact the developer": "联系开发者",
        "api version": "api版本",
        "Response Content Type": "响应Content Type",
        "fetching resource": "正在获取资源",
        "fetching resource list": "正在获取资源列表",
        "Explore": "浏览",
        "Show Swagger Petstore Example Apis": "显示 Swagger Petstore 示例 Apis",
        "Can't read from server.  It may not have the appropriate access-control-origin settings.": "无法从服务器读取。可能没有正确设置access-control-origin。",
        "Please specify the protocol for": "请指定协议：",
        "Can't read swagger JSON from": "无法读取swagger JSON于",
        "Finished Loading Resource Information. Rendering Swagger UI": "已加载资源信息。正在渲染Swagger UI",
        "Unable to read api": "无法读取api",
        "from path": "从路径",
        "Click to set as parameter value": "点击设置参数",
        "server returned": "服务器返回"
    },

    //定时执行转换
     _translator2Cn = function () {
         if ($("#resources_container .resource").length > 0) {
             _tryTranslate();
         }

         if ($("#explore").text() == "Explore" && iexcute < 500) {
             iexcute++;
             setTimeout(_translator2Cn, 50);
         }
     },

     //设置控制器注释
     _setControllerSummary = function () {
         $.ajax({
             type: "get",
             async: true,
             url: $("#input_baseUrl").val(),
             dataType: "json",
             success: function (data) {
                 var summaryDict = data.ControllerDesc;
                 var id, controllerName, strSummary;
                 $("#resources_container .resource").each(function (i, item) {
                     id = $(item).attr("id");
                     if (id) {
                         controllerName = id.substring(9);
                         strSummary = summaryDict[controllerName];
                         if (strSummary) {
                             $(item).children(".heading").children(".options").prepend('<li class="controller-summary" title="' + strSummary + '">' + strSummary + '</li>');
                         }
                     }
                 });
             }
         });
     },

     //尝试将英文转换成中文
    _tryTranslate = function () {
        $('[data-sw-translate]').each(function () {
            $(this).html(_getLangDesc($(this).html()));
            $(this).val(_getLangDesc($(this).val()));
            $(this).attr('title', _getLangDesc($(this).attr('title')));
        });
    },
    _getLangDesc = function (word) {
        return _words[$.trim(word)] !== undefined ? _words[$.trim(word)] : word;
    };

    return {
        Translator: function () {
            document.title = "政策接口API";
            $('body').append('<style type="text/css">.controller-summary{color:#10a54a !important;word-break:keep-all;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;max-width:250px;text-align:right;cursor:default;} </style>');
            $("#logo").html("API文档").attr("href", "/Home/Index");
            //设置控制器描述
            _setControllerSummary();
            _translator2Cn();
        }
    }
})();
//执行转换
SwaggerTranslator.Translator();


//汉化2
//'use strict';
///**
// * Translator for documentation pages.
// *
// * To enable translation you should include one of language-files in your index.html
// * after <script src='lang/translator.js' type='text/javascript'></script>.
// * For example - <script src='lang/ru.js' type='text/javascript'></script>
// *
// * If you wish to translate some new texsts you should do two things:
// * 1. Add a new phrase pair ("New Phrase": "New Translation") into your language file (for example lang/ru.js). It will be great if you add it in other language files too.
// * 2. Mark that text it templates this way <anyHtmlTag data-sw-translate>New Phrase</anyHtmlTag> or <anyHtmlTag data-sw-translate value='New Phrase'/>.
// * The main thing here is attribute data-sw-translate. Only inner html, title-attribute and value-attribute are going to translate.
// *
// */
//window.SwaggerTranslator = {
//    _words: [],

//    translate: function () {
//        var $this = this;
//        $('[data-sw-translate]').each(function () {
//            $(this).html($this._tryTranslate($(this).html()));
//            $(this).val($this._tryTranslate($(this).val()));
//            $(this).attr('title', $this._tryTranslate($(this).attr('title')));
//        });
//    },

//    _tryTranslate: function (word) {
//        return this._words[$.trim(word)] !== undefined ? this._words[$.trim(word)] : word;
//    },

//    learn: function (wordsMap) {
//        this._words = wordsMap;
//    }
//};


///* jshint quotmark: double */
//window.SwaggerTranslator.learn({
//    "Warning: Deprecated": "警告：已过时",
//    "Implementation Notes": "实现备注",
//    "Response Class": "响应类",
//    "Status": "状态",
//    "Parameters": "参数",
//    "Parameter": "参数",
//    "Value": "值",
//    "Description": "描述",
//    "Parameter Type": "参数类型",
//    "Data Type": "数据类型",
//    "Response Messages": "响应消息",
//    "HTTP Status Code": "HTTP状态码",
//    "Reason": "原因",
//    "Response Model": "响应模型",
//    "Request URL": "请求URL",
//    "Response Body": "响应体",
//    "Response Code": "响应码",
//    "Response Headers": "响应头",
//    "Hide Response": "隐藏响应",
//    "Headers": "头",
//    "Try it out!": ">>试一下！",
//    "Show/Hide": "显示/隐藏",
//    "List Operations": "显示操作",
//    "Expand Operations": "展开操作",
//    "Raw": "原始",
//    "can't parse JSON.  Raw result": "无法解析JSON. 原始结果",
//    "Model Schema": "模型架构",
//    "Model": "模型",
//    "Example Value":"范例",
//    "apply": "应用",
//    "Username": "用户名",
//    "Password": "密码",
//    "Terms of service": "服务条款",
//    "Created by": "创建者",
//    "See more at": "查看更多：",
//    "Contact the developer": "联系开发者",
//    "api version": "api版本",
//    "Response Content Type": "响应Content Type",
//    "fetching resource": "正在获取资源",
//    "fetching resource list": "正在获取资源列表",
//    "Explore": "浏览",
//    "Show Swagger Petstore Example Apis": "显示 Swagger Petstore 示例 Apis",
//    "Can't read from server.  It may not have the appropriate access-control-origin settings.": "无法从服务器读取。可能没有正确设置access-control-origin。",
//    "Please specify the protocol for": "请指定协议：",
//    "Can't read swagger JSON from": "无法读取swagger JSON于",
//    "Finished Loading Resource Information. Rendering Swagger UI": "已加载资源信息。正在渲染Swagger UI",
//    "Unable to read api": "无法读取api",
//    "from path": "从路径",
//    "server returned": "服务器返回"
//});


//$(function () {
//    window.SwaggerTranslator.translate();
//});