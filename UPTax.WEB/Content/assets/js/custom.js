﻿!function (i) { "use strict"; function e() { } e.prototype.init = function () { i(".selectize").selectize({ placeholder: "Select..."}),i("#selectize-tags").selectize({delimiter:",",persist:!1,create:function(e){return{value:e,text:e}}});var n={};i("input#defaultconfig").maxlength({warningClass:"badge badge-info",limitReachedClass:"badge badge-warning"}),i("input#thresholdconfig").maxlength({threshold:20,warningClass:"badge badge-info",limitReachedClass:"badge badge-warning"}),i("input#moreoptions").maxlength({alwaysShow:!0,warningClass:"badge badge-success",limitReachedClass:"badge badge-danger"}),i("input#alloptions").maxlength({alwaysShow:!0,warningClass:"badge badge-success",limitReachedClass:"badge badge-danger",separator:" out of ",preText:"You typed ",postText:" chars available.",validate:!0}),i("textarea#textarea").maxlength({alwaysShow:!0,warningClass:"badge badge-info",limitReachedClass:"badge badge-warning"}),i("input.placement").maxlength({alwaysShow:!0,placement:"top-left",warningClass:"badge badge-info",limitReachedClass:"badge badge-warning"})},i.AdvancedForm=new e,i.AdvancedForm.Constructor=e}(window.jQuery),function(){"use strict";window.jQuery.AdvancedForm.init()}();