// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
    $(document).ready(function () {
        // When the button is clicked
        $('.btn[data-action="toggle-drawer"]').click(function () {
            // Toggle aria-expanded attribute
            var ariaExpanded = $(this).attr("aria-expanded");
            $(this).attr(
                "aria-expanded",
                ariaExpanded === "false" ? "true" : "false"
            );

            // Add or remove class on body
            $("body").toggleClass("drawer-open-left");
            if (!$("body").hasClass("drawer-ease")) {
                $("body").addClass("drawer-ease");
            }

            // Toggle classes and attributes on the drawer element
            var drawer = $("#nav-drawer");
            drawer.toggleClass("closed");
            drawer.attr(
                "aria-hidden",
                drawer.hasClass("closed") ? "true" : "false"
            );
        });
    });