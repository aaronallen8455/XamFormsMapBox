using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mapbox.Maps;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using MapBox.Droid;
using Mapbox;
using Mapbox.Annotations;
using Mapbox.Geometry;
using Mapbox.Camera;
using MapBox;

[assembly: ExportRenderer(typeof(MapBoxControl), typeof(MapBoxRenderer))]
namespace MapBox.Droid
{
    class MapBoxRenderer : ViewRenderer<MapBoxControl, MapView>, IOnMapReadyCallback
    {
        private MapView Map;

        public MapBoxRenderer(Context context) : base(context) { }

        protected override async void OnElementChanged(ElementChangedEventArgs<MapBoxControl> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                //MapboxAccountManager.Start(Context, "pk.eyJ1IjoidmVzc2VsYXBwIiwiYSI6ImNqZml0ems1YzBndDcyenFxdTFmbXFya3EifQ.O2v_RvniaVAkYE3rQVjw3A");

                MapboxMapOptions options = new MapboxMapOptions();
                options.InvokeScrollGesturesEnabled(true);
                options.InvokeRotateGesturesEnabled(true);
                options.InvokeTiltGesturesEnabled(true);
                options.InvokeZoomGesturesEnabled(true);

                Map = new MapView(Context, options);
                Map.StyleUrl = "mapbox://styles/vesselapp/cjfj2i5rgcq8t2rqgj5spfzvk";
                Map.OnCreate(null);
                Map.GetMap(this);

                SetNativeControl(Map);
            }
        }

        public void OnMapReady(MapboxMap map)
        {
            var uiSettings = map.UiSettings;
            uiSettings.SetAllGesturesEnabled(true);

            map.AddMarker(new MarkerOptions()
                .SetTitle("Austin")
                .SetSnippet("Texas, Baby")
                .SetPosition(new LatLng(30.266461, -97.748630)));

            var bounds = new LatLngBounds.Builder()
                .Include(new LatLng(30.266461, -97.748630))
                .Include(new LatLng(30, -97))
                .Build();

            map.InfoWindowClick += Map_InfoWindowClick;
            map.CameraChange += Map_CameraChange;

            map.MoveCamera(CameraUpdateFactory.NewLatLngBounds(bounds, 8));
        }

        /// <summary>
        /// Can be used to detect zoom changes, perform pin clustering
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Map_CameraChange(object sender, MapboxMap.CameraChangeEventArgs e)
        {

        }

        private void Map_InfoWindowClick(object sender, MapboxMap.InfoWindowClickEventArgs e)
        {

        }
    }
}