using MapKit;

namespace MSCorp.FirstResponse.Client.iOS.Maps.Annotations
{
    public class UserAnnotationView : MKAnnotationView
    {
        public const string CustomReuseIdentifier = nameof(UserAnnotationView);

        public UserAnnotationView(IMKAnnotation annotation)
            : base(annotation, CustomReuseIdentifier)
        {
            Image = AnnotationImages.UserImage;
        }
    }
}