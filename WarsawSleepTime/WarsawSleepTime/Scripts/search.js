var map;
var infowindow;

$(function () {
    initMap();
    initButtonDependencies();
});

var dataMap;

// ReSharper disable once NativeTypePrototypeExtending
String.prototype.isEmpty = function () {
    return (this.length === 0 || !this.trim());
};

function initButtonDependencies() {
    $("#searchButton").click(function () {
        var street = $("#street").val();
        var number = $("#number").val();
        var date = $("#date").val();
        var dateSpecified = true;
        // ReSharper disable once PossiblyUnassignedProperty
        var data = new Date(date);
        if (!data.isValid && date !== "") {
            data = new Date();
        }
        if (date === "") {
            dateSpecified = false;
        }



        var hotels = $("#hotelsCheck").is(':checked') ? true : false;
        var hostels = $("#hostelsCheck").is(':checked') ? true : false;
        var dormitories = $("#dormitoriesCheck").is(':checked') ? true : false;
        var free = $("#freeCheck").is(':checked') ? true : false;
        var preferences = $("#preferencesCheck").is(':checked') ? true : false;
        var bounds = map.getBounds();
        //var southWest = {bounds.H.H, bounds.j.H};//.getSouthWest();
        //var northEast = bounds.j;//.getNorthEast();
        $.ajax({
            url: "/Offers/SearchigResults",
            type: 'POST',
            dataType: 'json',
            cache: false,
            data: {
                criteria: {
                    Paid: hotels,
                    Hostels: hostels,
                    Dormitories: dormitories,
                    Free: free,
                    Street: street,
                    Number: number,
                    MatchPreferences: preferences,
                    Date: data,
                    DateSpecified: dateSpecified,
                    southWestX: bounds.j.j,
                    southWestY: bounds.H.j,
                    northEastX: bounds.j.H,
                    northEastY: bounds.H.H,
                    zoom: map.getZoom()
                }
            },
            success: function (results) {
                refreshMap(results);
            },
            error: function (ts) {
                alert(ts.responseText);
            }
        });

    });
}

var markers = [];
var selectedMarker;

function hideMarkers() {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(null);
    }
    markers = [];
}
function func(id) {
    window.location.replace(location.origin + "/Offers/Offer/" + id);
};

function refreshMap(data) {
    hideMarkers();

    for (var i = 0; i < data.FreeOffers.length; i++) {
        var ind = i;
        var marker = new google.maps.Marker({
            position: { lat: data.FreeOffers[ind].Latitude, lng: data.FreeOffers[ind].Longtitude },
            map: map,
            title: 'Warsaw Sleep Time',
            icon: 'https://maps.gstatic.com/mapfiles/ms2/micons/ltblue-dot.png'
        });
        markers.push(marker);
        marker.setMap(map);
        marker.content = data.FreeOffers[ind];
        google.maps.event.addListener(marker, 'click', function () {
            var self = this;
            var contentString = '<div><div id="owner">' + self.content.Owner + '</div><button id="ajdik" onclick=\"func(' + self.content.Id + ');\">See details</button></div>';
            if (infowindow) {
                infowindow.close();
            }
            infowindow = new google.maps.InfoWindow({
                content: contentString
            });

            infowindow.open(map, this);
        });

    }
    for (var i = 0; i < data.PaidOffers.length; i++) {
        var ind = i;
        var marker = new google.maps.Marker({
            position: { lat: data.PaidOffers[ind].Latitude, lng: data.PaidOffers[ind].Longtitude },
            map: map,
            title: 'Warsaw Sleep Time',
            icon: 'https://www.google.com/intl/en_us/mapfiles/ms/micons/green-dot.png'
        });
        markers.push(marker);
        marker.setMap(map);
        marker.content = data.PaidOffers[ind];
        google.maps.event.addListener(marker, 'click', function () {
            var self = this;
            var contentString = '<div><div id="owner">' + self.content.Owner + '</div><div>' + self.content.Url + '</div><div><a href=\"https://www.google.pl/search?q=' + self.content.Owner + '\">Find in google</a></div></div>';
            if (infowindow) {
                infowindow.close();
            }
            infowindow = new google.maps.InfoWindow({
                content: contentString
            });

            infowindow.open(map, this);
        });
    }


    for (var i = 0; i < data.Dormitories.length; i++) {
        var ind = i;
        var marker = new google.maps.Marker({
            position: { lat: data.Dormitories[ind].Latitude, lng: data.Dormitories[ind].Longtitude },
            map: map,
            title: 'Warsaw Sleep Time',
            icon: 'http://www.armeniapedia.org/images/7/79/Marker-orange.png'
        });
        markers.push(marker);
        marker.setMap(map);
        marker.content = data.Dormitories[ind];
        google.maps.event.addListener(marker, 'click', function () {
            var self = this;
            var contentString = '<div><div id="owner">' + self.content.Owner + '</div><div>' + self.content.Url + '</div><div><a href=\"https://www.google.pl/search?q=' + self.content.Owner + '\">Find in google</a></div></div>';
            if (infowindow) {
                infowindow.close();
            }
            infowindow = new google.maps.InfoWindow({
                content: contentString
            });

            infowindow.open(map, this);
        });
    }
    for (var i = 0; i < data.Hostels.length; i++) {
        var ind = i;
        var marker = new google.maps.Marker({
            position: { lat: data.Hostels[ind].Latitude, lng: data.Hostels[ind].Longtitude },
            map: map,
            title: 'Warsaw Sleep Time',
            icon: 'http://maps.google.com/mapfiles/ms/micons/pink-dot.png'
        });
        markers.push(marker);
        marker.setMap(map);
        marker.content = data.Hostels[ind];
        google.maps.event.addListener(marker, 'click', function () {
            var self = this;
            var contentString = '<div><div id="owner">' + self.content.Owner + '</div><div>' + self.content.Url + '</div><div><a href=\"https://www.google.pl/search?q=' + self.content.Owner + '\">Find in google</a></div></div>';
            if (infowindow) {
                infowindow.close();
            }
            infowindow = new google.maps.InfoWindow({
                content: contentString
            });

            infowindow.open(map, this);
        });
    }

}



function initMap() {

    var point;
    var zoomValue = 12;
    if (map == null) {
        point = { lat: 52.222202, lng: 21.006968 };
    } else {
        var bounds = map.getBounds();
        if (bounds) {
            var southWest = bounds.getSouthWest();
            var northEast = bounds.getNorthEast();
            point = { lng: (northEast.lng() + southWest.lng()) / 2, lat: (northEast.lat() + southWest.lat()) / 2 };
            zoomValue = map.getZoom();
        } else {
            point = { lat: 52.222202, lng: 21.006968 };
        }
    }
    var mapDiv = document.getElementById("minimap");
    // ReSharper disable once UseOfImplicitGlobalInFunctionScope
    map = new google.maps.Map(mapDiv, {
        center: point,
        zoom: zoomValue
    });
    google.maps.event.addListener(map, 'idle', function () {
        $("#searchButton").click();
    });
}
