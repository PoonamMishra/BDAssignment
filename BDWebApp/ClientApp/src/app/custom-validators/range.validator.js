"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var RangeValidator = /** @class */ (function () {
    function RangeValidator() {
    }
    RangeValidator.validateRange = function (control) {
        if (control.value != null && (control.value > 10 || control.value < 1)) {
            return { 'invalidRange': true };
        }
        return false;
    };
    return RangeValidator;
}());
exports.RangeValidator = RangeValidator;
//# sourceMappingURL=range.validator.js.map