﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
/*--------------------------------------------------*/
/*                 Loading Mask                     */
/*--------------------------------------------------*/
LoadingMask = {
    show: function () {
        //$.blockUI({ message: '<div class="boxes"><div class="box"><div></div><div></div><div></div><div></div></div><div class="box"><div></div><div></div><div></div><div></div></div><div class="box"><div></div><div></div><div></div><div></div></div><div class="box"><div></div><div></div><div></div><div></div></div></div>' });
        
        $.blockUI({
            css: {
                padding: '15px',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff',
                backgroundColor: 'transparent',
                border: '0'
            },
            message: '<div class="sk-cube-grid"><div class="sk-cube sk-cube1"></div><div class="sk-cube sk-cube2"></div><div class="sk-cube sk-cube3"></div><div class="sk-cube sk-cube4"></div><div class="sk-cube sk-cube5"></div><div class="sk-cube sk-cube6"></div><div class="sk-cube sk-cube7"></div><div class="sk-cube sk-cube8"></div><div class="sk-cube sk-cube9"></div></div>'
        });        
    },
    hide: function () {
        $.unblockUI();
    }
}

post_to_url = function (path, params, method) {
    method = method || "post";

    var form = document.createElement("form");
    form.setAttribute("method", method);
    form.setAttribute("action", path);

    for (var key in params) {
        if (params.hasOwnProperty(key) && params[key] != null) {
            var hiddenField = document.createElement("input");
            hiddenField.setAttribute("type", "hidden");
            hiddenField.setAttribute("name", key);
            hiddenField.setAttribute("value", params[key]);

            form.appendChild(hiddenField);
        }
    }

    document.body.appendChild(form);
    form.submit();
} 