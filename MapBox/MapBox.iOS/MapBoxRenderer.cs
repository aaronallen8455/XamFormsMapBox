using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Mapbox;
using MapBox;
using MapBox.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MapBoxControl), typeof(MapBoxRenderer))]
namespace MapBox.iOS
{
    public class MapBoxRenderer : ViewRenderer<MapBoxControl, MapView>
    {
		protected override void OnElementChanged(ElementChangedEventArgs<MapBoxControl> e)
		{
            base.OnElementChanged(e);

            if (Control == null)
            {
                var mapView = new MapView();
                mapView.StyleURL = new Foundation.NSUrl("mapbox://styles/vesselapp/cjfj2i5rgcq8t2rqgj5spfzvk");
                mapView.CenterCoordinate = new CoreLocation.CLLocationCoordinate2D(30.266461, -97.748630);
                mapView.ZoomLevel = 2;
                mapView.Delegate = new MVDelegate();

                SetNativeControl(mapView);
            }
		}

        class MVDelegate : MapViewDelegate
        {
			public override void DidFinishLoadingMap(MapView mapView)
			{
                var annotation = new PointAnnotation();
                annotation.Coordinate = new CoreLocation.CLLocationCoordinate2D(30.266461, -97.748630);
                annotation.Title = "Austin";
                annotation.Subtitle = "Texas";

                mapView.AddAnnotation(annotation);
			}

			public override AnnotationView GetAnnotationView(MapView mapView, IAnnotation annotation)
			{
                if (!(annotation is PointAnnotation)) return null;

                string reuseIdentifier = ((PointAnnotation)annotation).Coordinate.Longitude.ToString();
                // for better performance, reuse views
                var annotationView = mapView.DequeueReusableAnnotationView(reuseIdentifier);

                if (annotationView == null)
                {
                    annotationView = new CustomAnnotationView(reuseIdentifier);
                    annotationView.Frame = new CGRect(0, 0, 48, 48);

                    // Set the annotation view's background to a value determined by its longitude
                    float hue = Math.Abs((float)((PointAnnotation)annotation).Coordinate.Longitude / 100);
                    annotationView.BackgroundColor = UIColor.FromHSBA(hue, .5f, 1f, 1f);
                }

                return annotationView;
			}

			public override bool CanShowCallout(MapView mapView, IAnnotation annotation)
			{
                return true;
			}
		}

        class CustomAnnotationView : AnnotationView
        {
            string _ReuseIdentifier;

            public CustomAnnotationView(string reuseIdentifier)
            {
                _ReuseIdentifier = reuseIdentifier;
            }

			public override void LayoutSubviews()
			{
                base.LayoutSubviews();

                // force the annotation view to maintain a constant size when the map is titled
                ScalesWithViewingDistance = false;

                // Use CALayers corner radius to turn this view into a circle.
                Layer.CornerRadius = Frame.Width / 2;
                Layer.BorderColor = UIColor.White.CGColor;
			}

            public override bool Selected 
            { 
                get => base.Selected; 
                set
                {
                    base.Selected = value;

                    // Animate the border with in/out, creating an iris effect.
                    var animation = new CABasicAnimation();
                    animation.KeyPath = "borderWidth";
                    animation.Duration = .1;
                    Layer.BorderWidth = Selected ? Frame.Width / 4 : 2;
                    Layer.AddAnimation(animation, "borderWidth");
                }
            }

            public override string ReuseIdentifier => _ReuseIdentifier;
		}
	}
}
