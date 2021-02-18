const ctx = {
	noriHostDiv: null,
	uiRoot: null,
	rootElementId: null,
	rootElement: null,
	elementById: {},
	frameSize: [0, 0],
	textSizer: null,
	fontsLoading: [],
	
	// some static data:
	isTextProperty: {
		// Note: color properties count as text properties.
		'btn.text': true,
		'btn.gradtop': true,
		'btn.gradbottom': true,
		'txtblk.text': true,
		'border.leftcolor': true,
		'border.topcolor': true,
		'border.rightcolor': true,
		'border.bottomcolor': true,
		'el.bgcolor': true,
		'el.fontcolor': true,
		'el.font': true,
		'el.shadowcolor': true,
		'float.anchorleft': true,
		'float.anchortop': true,
		'float.anchorright': true,
		'float.anchorbottom': true,
		'img.src': true,
		'input.value': true,
	},
	layoutExemptProperties: {
		'el.bgcolor': true,
		'el.cursor': true,
		'el.focus': true,
		'el.fontcolor': true,
		'el.onmousedown': true,
		'el.onclick': true,
		'el.onmouseup': true,
		'el.onmousemove': true,
		'el.onmouseenter': true,
		'el.onmouseleave': true,
		'el.onscrollintoview': true,
		'el.opacity': true,
		'el.shadowblur': true,
		'el.shadowcolor': true,
		'el.shadowx': true,
		'el.shadowy': true,
		'border.radius': true,
		'border.leftcolor': true,
		'border.topcolor': true,
		'border.rightcolor': true,
		'border.bottomcolor': true,
		'btn.gradtop': true,
		'btn.gradbottom': true,
		'btn.radius': true,
		'cv.height': true,
		'cv.width': true,
		'input.onfocus': true,
		'input.onblur': true,
		'input.changed': true,
		'tb.multiline': true,
	},
	isPanelType: {
		'Border': true,
		'DockPanel': true,
		'FloatPanel': true,
		'FlowPanel': true,
		'ScrollPanel': true,
		'StackPanel': true,
	},
	scrollEnumLookup: ['none', 'auto', 'scroll', 'crop'],

	hasKeyDownListener: false,
	hasKeyUpListener: false,
};
