/*global pictoBackbone, Backbone, JST*/

pictoBackbone.Views = pictoBackbone.Views || {};

(function () {
    'use strict';

    pictoBackbone.Views.ApplicationView = Backbone.View.extend({

        template: JST['app/scripts/templates/application.ejs']

    });

})();
