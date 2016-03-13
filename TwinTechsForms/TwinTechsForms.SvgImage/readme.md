# TwinTechsForms.SvgImage

## Background

This project is a proof-of-concept of paulpatarinski's SvgImage, but running with a non-custom NGraphics reference, and allowing for stretchable (9-slice) SVGs.

We used the original SvgImage all over the place, and it worked great for most of our SVG needs. The original SvgImage uses a custom fork of NGraphics that has lagged behind NGraphic's quickly-evolving SVG support. While we were working toward supporting 9-slice SVG support, we forked from SvgImage and brought it back to using the raw NGraphics directly.

While making this transition, the code responsible for drawing was moved from the platform renderer's `OnElementChanged` methods to the `Draw` method, triggered by property changes, with most of the duplicated drawing code migrated back the shared PCL code. Besides consolidating code, this change allows changes to SvgImage properties at runtime to be reflected in the UI.

For the full background on this fork of SvgImage, check out the [Twin blog post about creating 9-slice SVGs](http://blog.twintechs.com/explorations-in-cross-platform-assets-9-slice-stretchable-svgs-in-xamarin.forms-apps).

## Getting TwinTechsForms.SvgImage

You can currently clone/download this repo and integrate it into your project to start using it. Soon, though, this version of SvgImage will be available via NuGet.

## 9-Slice SvgImage Use

Since we carried on the original SvgImage conventions, you will still need to add the initialization call to your platform projects right after Xamarin.Forms is initialized.

    using TwinTechs;
    
    //…
    
    Xamarin.Forms.Init();
    SvgImageRenderer.Init();

From there, you can add `SvgImage`s to your Xamarin.Forms application UI (customing `YourApp` and value for `SvgPath` with your application class and path, respectively).

    new SvgImage() {
        SvgAssembly = typeof(YourApp).GetTypeInfo().Assembly,
        SvgPath = "Path.To.Your.Embedded.Resource.svg",
        WidthRequest = 300,
        HeightRequest = 300,
        SvgStretchableInsets = new ResizableSvgInsets(30),
    };

## 9-Slice Considerations

While you can set the stretchable insets to just about anything you want, if the values result in overlaps on the original SVG image, weird things will happen. (Spoiler: portions of the rendered image will be repeated or you will cause an exception, neither ideal.)

In the demonstration example app, you can adjust the slider at while and the SVG will resample with the new inset values. Currently, doing so on Android will quickly reveal a memory leak in the NGraphics SvgReader class. If you plan to change properties quickly on your SvgImage, you probably want to cache things at some level to avoid hitting the SvgReader work at all.

## Why Not Contribute Back?

Putting these changes into a pull request to the original SvgImage is definitely something we want to do. Unfortunately, to match what SvgImage offers currently, we would need to add Windows support, which is going to require more time than we can commit to it right now. Until then, we wanted to share the progress we have on 9-slice SVG support so others can take advantage of the new possibilities in cross-platform image sharing. 

## Coming Soon

SvgImage use for buttons without a transparent button overlay. If you just can't wait and want that workaround to use now, check out the [Twin blog post about creating 9-slice SVGs (toward the bottom)](http://blog.twintechs.com/explorations-in-cross-platform-assets-xamarin-forms).
