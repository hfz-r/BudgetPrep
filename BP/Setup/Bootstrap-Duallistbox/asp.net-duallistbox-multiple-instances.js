function InitDualDropList() {

    $('div.dummy > select').each(function (index, element) {
        $(element).bootstrapDualListbox({
            moveOnSelect: false,
            preserveSelectionOnMove: 'moved',
            helperSelectNamePostfix: '_helper',
            index: index
        })
        //this is the global setting
        //.bootstrapDualListbox('setFilterPlaceHolder', 'Filter')
		//.bootstrapDualListbox('setMoveAllLabel', 'Move all')
		//.bootstrapDualListbox('setRemoveAllLabel', 'Remove all')
        //.bootstrapDualListbox('setMoveSelectedLabel', 'Move selected')
        //.bootstrapDualListbox('setRemoveSelectedLabel', 'Remove selected')
        //.bootstrapDualListbox('setMoveOnSelect', false)
        //.bootstrapDualListbox('setPreserveSelectionOnMove', 'moved')
        //.bootstrapDualListbox('setHelperSelectNamePostfix', '_helper')
        //.bootstrapDualListbox('setSelectOrMinimalHeight', undefined)
        //.bootstrapDualListbox('setSelectedFilter', undefined)
		//.bootstrapDualListbox('setNonSelectedFilter', undefined)
        .bootstrapDualListbox('refresh')
    });
}