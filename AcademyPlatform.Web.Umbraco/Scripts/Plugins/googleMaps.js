var simplerweb = new google.maps.LatLng(42.688234, 23.359331);
var marker;
var windowWidth = jQuery(window).width();
var map;
var styles = [{
    stylers: [
        { hue: "#61504e" },
        { saturation: -30 }
    ]
}, {
    featureType: "road",
    elementType: "geometry",
    stylers: [
        { lightness: 100 },
        { visibility: "simplified" }
    ]
}, {
    featureType: "road",
    elementType: "labels",
    stylers: [
        { hue: "#5b4d4c" }
    ]
}];

function initialize() {
    var myOpts = {
        center: simplerweb,
        zoom: 16,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var map = new google.maps.Map(document.getElementById("map"), myOpts);
    marker = new google.maps.Marker({
        map: map,
        // draggable:true,
        // animation: google.maps.Animation.DROP,
        position: new google.maps.LatLng(42.688234, 23.359331),
        icon: '/media/1128/map-marker.png' // null = default icon
    });
    
    //map.setOptions({styles: styles});
    map.setOptions({
        draggable: true,
        zoomControl: false,
        scrollwheel: false,
        disableDoubleClickZoom: false,
        truescrollwheel: false,
        navigationControl: false,
        mapTypeControl: false,
        scaleControl: false
    });
    if (windowWidth <= 500) {
        map.setOptions({
            draggable: false,
            disableDoubleClickZoom: true
        });

    }
    google.maps.event.addListener(marker, 'click', toggleBounce);
}

function toggleBounce() {
    if (marker.getAnimation() != null) {
        marker.setAnimation(null);
    } else {
        marker.setAnimation(google.maps.Animation.BOUNCE);
    }
}

google.maps.event.addDomListener(window, 'load', initialize);