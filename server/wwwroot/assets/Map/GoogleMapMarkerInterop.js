﻿window.blazorGoogleMap = window.blazorGoogleMap || {};
window.blazorGoogleMap.markersModule = {
    markers: [],

    addMarker: function (markerRef, marker, markerId) {
        var that = blazorGoogleMap.markersModule;
        var mapMarker = new google.maps.Marker({
            map: blazorGoogleMap.map,
            position: marker.position,
            title: marker.title,
            animation: marker.animation,
            label: marker.label,
            icon: marker.icon,
            opacity: marker.opacity,
            draggable: marker.draggable,
            clickable: marker.clickable,
            cursor: marker.cursor
        });

        mapMarker.addListener('dragend', function (e) {
            return markerRef.invokeMethodAsync('OnDragEndHandle', e);
        });

        if (marker.onClick !== null) {
            mapMarker.addListener('click', function () {
                return markerRef.invokeMethodAsync('OnClickHandle');
            });
        }

        if (marker.onMarkerRightClick !== null) {
            mapMarker.addListener('rightclick', function () {
                return markerRef.invokeMethodAsync('OnRightClickHandle');
            });
        }

        if (marker.onMarkerDblClick !== null) {
            mapMarker.addListener('dblclick', function () {
                return markerRef.invokeMethodAsync('OnDoubleClickHandle');
            });
        }

        mapMarker.id = markerId;
        that.markers.push({ mapMarker: mapMarker, markerRef: markerRef });

        return true;
    },

    removeMarker: function (markerId) {
        var markerPair = blazorGoogleMap.markersModule.markers.find(function (m) {
            return m.mapMarker.id === markerId;
        });

        if (markerPair === undefined) {
            return false;
        }

        markerPair.mapMarker.setMap(null);
        const filteredMarkers = blazorGoogleMap.markersModule.markers.filter(
            function (marker) {
                return marker.id !== markerId;
            }
        );

        var removeResult = filteredMarkers.length === blazorGoogleMap.markersModule.markers.length - 1;
        blazorGoogleMap.markersModule.markers = filteredMarkers;

        return removeResult;
    },

    setAnimation: function (markerId, animation) {
        var marker =blazorGoogleMap.markersModule.findMarker(markerId);
        if (marker !== undefined) {
            marker.setAnimation(animation);
        }
    },

    setIcon: function (markerId, icon) {
        var marker = blazorGoogleMap.markersModule.findMarker(markerId);
        if (marker !== undefined) {
            marker.setIcon(icon);
        }
    },

    setOpacity: function (markerId, opacity) {
        var marker = blazorGoogleMap.markersModule.findMarker(markerId);
        if (marker !== undefined) {
            marker.setOpacity(opacity);
        }
    },

    setVisibility: function (markerId, visible) {
        var marker = blazorGoogleMap.markersModule.findMarker(markerId);
        if (marker !== undefined) {
            marker.setVisible(visible);
        }
    },

    findMarker: function (markerId) {
        var that = this === window
            ? blazorGoogleMap.markersModule
            : this;

        var marker = that.markers.find(function (m) {
            return m.mapMarker.id === markerId;
        });

        return marker.mapMarker;
    }
};