Ext.net.FilterHeader.behaviour.string[0].match = function (recordValue, matchValue) {
    return (Ext.net.FilterHeader.behaviour.getStrValue(recordValue) || "").indexOf(matchValue) > -1;
};


Ext.net.FilterHeader.behaviour.string[0].serialize = function (value) {
    return {
        type: "string",
        op: "*",
        value: value
    };
};

var TwoGridSelector = {
    add: function (source, destination) {
        if (source.selModel.hasSelection()) {
            var records = source.selModel.getSelection();
            source.store.remove(records);
            destination.store.add(records);
        }
    },
    addAll: function (source, destination) {
        var records = source.store.getRange();
        source.store.removeAll();
        destination.store.add(records);
    },
    remove: function (source, destination) {
        this.add(destination, source);
    },
    removeAll: function (source, destination) {
        this.addAll(destination, source);
    }
};