using System.Web;
using System.Web.Optimization;

namespace UPTax
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/bower_components/jquery/dist/jquery.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/bower_components/bootstrap/dist/js/bootstrap.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/otherjs").Include(
                      "~/bower_components/jquery-ui/jquery-ui.min.js",
                      "~/bower_components/raphael/raphael.min.js",
                      "~/bower_components/morris.js/morris.min.js",
                      "~/bower_components/jquery-sparkline/dist/jquery.sparkline.min.js",
                      "~/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                      "~/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                      "~/bower_components/jquery-knob/dist/jquery.knob.min.js",
                      "~/bower_components/moment/min/moment.min.js",
                      "~/bower_components/bootstrap-daterangepicker/daterangepicker.js",
                      "~/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js",
                      "~/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                      "~/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                      "~/bower_components/fastclick/lib/fastclick.js",
                      "~/bower_components/datatables.net/js/jquery.dataTables.min.js",
                      "~/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js",
                      "~/dist/js/adminlte.min.js",
                      "~/bower_components/chart.js/Chart.js",
                      "~/Scripts/jquery.unobtrusive-ajax.min.js",
                      "~/dist/js/demo.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/bower_components/bootstrap/dist/css/bootstrap.min.css",
                      "~/bower_components/font-awesome/css/font-awesome.min.css",
                      "~/bower_components/Ionicons/css/ionicons.min.css",
                      "~/bower_components/morris.js/morris.css",
                      "~/bower_components/jvectormap/jquery-jvectormap.css",
                      "~/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css",
                      "~/bower_components/bootstrap-daterangepicker/daterangepicker.css",
                      "~/bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css",
                      "~/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css",
                      "~/dist/css/AdminLTE.min.css",
                      "~/dist/css/skins/_all-skins.min.css",
                      "~/dist/css/style.css"
                      ));

            bundles.Add(new StyleBundle("~/Login/Css").Include(
                        "~/bower_components/bootstrap/dist/css/bootstrap.min.css",
                        "~/bower_components/font-awesome/css/font-awesome.min.css",
                        "~/bower_components/Ionicons/css/ionicons.min.css",
                        "~/dist/css/AdminLTE.min.css",
                        "~/plugins/iCheck/square/blue.css"
                        ));
            bundles.Add(new ScriptBundle("~/Login/Loginjs").Include(
                      "~/plugins/iCheck/icheck.min.js"
                      ));

            #region Front page css
            bundles.Add(new StyleBundle("~/Content/frontCss").Include(
                      "~/assets/css/bootstrap.min.css",
                      "~/venobox/venobox.css",
                      "~/assets/css/plugin_theme_css.css",
                      "~/assets/css/style.css",
                      "~/style.css",
                      "~/assets/css/responsive.css"
                      ));
            #endregion

            #region Front page JS
            bundles.Add(new ScriptBundle("~/bundles/frontjs").Include(
                      "~/assets/js/vendor/jquery-3.5.1.min.js",
                      "~/assets/js/bootstrap.min.js",
                      "~/assets/js/isotope.pkgd.min.js",
                      "~/assets/js/owl.carousel.min.js",
                      "~/assets/js/jquery.nivo.slider.pack.js",
                      "~/assets/js/slick.min.js",
                      "~/assets/js/imagesloaded.pkgd.min.js",
                      "~/venobox/venobox.min.js",
                      "~/assets/js/jquery.appear.js",
                      "~/assets/js/jquery.knob.js",
                      "~/assets/js/BeerSlider.js",
                      "~/assets/js/theme-pluginjs.js",
                      "~/assets/js/jquery.meanmenu.js",
                      "~/assets/js/ajax-mail.js",
                      "~/assets/js/theme.js"
                      ));
            #endregion

            #region Admin Dashboard CSS

            bundles.Add(new StyleBundle("~/Content/adminCss").Include(
                    "~/Content/assets/libs/air-datepicker/css/datepicker.min.css",
                    "~/Content/assets/libs/jqvmap/jqvmap.min.css",
                    "~/Content/assets/libs/alertifyjs/build/css/alertify.min.css",
                    "~/Content/assets/libs/alertifyjs/build/css/themes/default.min.css",
                    "~/Content/assets/css/bootstrap.min.css",
                    "~/Content/assets/css/icons.min.css",
                    "~/Content/assets/css/app.min.css",
                    "~/Content/assets/libs/selectize/css/selectize.css", 
                    "~/Content/style.css"
                    ));
            #endregion

            #region Admin Dashboard JS

            bundles.Add(new ScriptBundle("~/bundles/adminjs").Include(
                      "~/Content/assets/libs/jquery/jquery.min.js",

                      "~/Content/assets/libs/bootstrap/js/bootstrap.bundle.min.js",
                      "~/Content/assets/libs/metismenu/metisMenu.min.js",
                      "~/Content/assets/libs/simplebar/simplebar.min.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/adminjs2").Include(
                     "~/Scripts/jquery.unobtrusive-ajax.min.js",
                     "~/Content/assets/libs/air-datepicker/js/datepicker.min.js",
                     "~/Content/assets/libs/air-datepicker/js/i18n/datepicker.en.js",
                     "~/Content/assets/libs/jquery-knob/jquery.knob.min.js",
                     "~/Content/assets/libs/jqvmap/jquery.vmap.min.js",
                     "~/Content/assets/libs/jqvmap/maps/jquery.vmap.usa.js",
                     "~/Content/assets/libs/node-waves/waves.min.js",
                     "~/Content/assets/libs/alertifyjs/build/alertify.min.js",
                     "~/Content/assets/js/pages/alertifyjs.init.js",
                     "~/Content/assets/js/pages/dashboard.init.js",
                     "~/Content/assets/libs/parsleyjs/parsley.min.js",
                     "~/Content/assets/js/pages/form-validation.init.js",
                     "~/Content/assets/js/avro-v1.1.4.min.js",
                     "~/Content/assets/libs/selectize/js/standalone/selectize.min.js",
                     "~/Content/assets/js/custom.js",
                     "~/Content/assets/js/app.js"
                     ));
            #endregion

            #region Admin Login CSS
            bundles.Add(new StyleBundle("~/Content/adminLoginCss").Include(
                    "~/Content/assets/css/bootstrap.min.css",
                    "~/Content/assets/css/icons.min.css",
                    "~/Content/assets/css/app.min.css"
                    ));
            #endregion

            #region Admin Login JS
            bundles.Add(new ScriptBundle("~/bundles/adminLoginjs").Include(
                    "~/Content/assets/libs/jquery/jquery.min.js",
                    "~/Content/assets/libs/bootstrap/js/bootstrap.bundle.min.js",
                    "~/Content/assets/libs/metismenu/metisMenu.min.js",
                    "~/Content/assets/libs/simplebar/simplebar.min.js",
                    "~/Content/assets/libs/node-waves/waves.min.js"
                    ));
            #endregion

            #region Admin Databale CSS

            bundles.Add(new StyleBundle("~/Content/dataTableCss").Include(
                "~/Content/assets/libs/datatables.net-bs4/css/dataTables.bootstrap4.min.css",
                "~/Content/assets/libs/datatables.net-buttons-bs4/css/buttons.bootstrap4.min.css",
                "~/Content/assets/libs/datatables.net-responsive-bs4/css/responsive.bootstrap4.min.css"
                ));
            #endregion

            #region Admin Databale CSS

            bundles.Add(new ScriptBundle("~/bundles/dataTablejs").Include(
                    "~/Content/assets/libs/datatables.net/js/jquery.dataTables.min.js",
                    "~/Content/assets/libs/datatables.net-bs4/js/dataTables.bootstrap4.min.js",
                    "~/Content/assets/libs/datatables.net-buttons/js/dataTables.buttons.min.js",
                    "~/Content/assets/libs/datatables.net-buttons-bs4/js/buttons.bootstrap4.min.js",
                    "~/Content/assets/libs/datatables.net-buttons/js/buttons.html5.min.js",
                    "~/Content/assets/libs/datatables.net-buttons/js/buttons.print.min.js",
                    "~/Content/assets/libs/datatables.net-buttons/js/buttons.colVis.min.js",
                    "~/Content/assets/libs/datatables.net-responsive/js/dataTables.responsive.min.js",
                    "~/Content/assets/libs/datatables.net-responsive-bs4/js/responsive.bootstrap4.min.js",
                    "~/Content/assets/js/pages/datatables.init.js"
                    ));
            #endregion
        }
    }
}
