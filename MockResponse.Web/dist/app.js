"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var vue_1 = require("vue");
var v = new vue_1.default({
    el: "#app",
    template: "\n    <div>\n        <div>Hello {{name}}!</div>\n        Name: <input v-model=\"name\" type=\"text\">\n    </div>",
    data: {
        name: "World"
    }
});
//# sourceMappingURL=app.js.map