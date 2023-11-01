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
    $('.categoryname').click(function () {
        var $category = $(this).closest('.category');
        $category.toggleClass('collapsed');
        var isCollapsed = $category.hasClass('collapsed');
        $category.attr('aria-expanded', isCollapsed ? 'false' : 'true');
    });
});
const api = "https://localhost:7167/odata/";
var getStudentCourse = (id, token) =>
    $.ajax({
        url: api + `Students(${id})/CourseStudents?$expand=Course`,
        type: "GET",
        headers: {
            "Authorization": `Bearer ${token}`,
        },
        success: (data) => loadCoursesToNav(data["value"]),
        error: () => alert("error"),
    });

const loadCoursesToNav = (data) => {
    $("#drop-down-menu").empty();
    data.forEach((ele) => {
        let html = `
                <a class="dropdown-item" role="menuitem" href="/Course/Details?id=${ele.Course.CourseId}" title="">${ele.Course.CourseName}</a>`;
        $(`#drop-down-menu`).append(html);
        html = `<a class="list-group-item list-group-item-action " href="/Course/Details?id=${ele.Course.CourseId}" data-key="${ele.Course.CourseId}" data-isexpandable="1" data-indent="1" data-showdivider="0" data-type="20" data-nodetype="1" data-collapse="0" data-forceopen="0" data-isactive="0" data-hidden="0" data-preceedwithhr="0" data-parent-key="mycourses">
            <div class="m-l-1" >
                <div class="media">
                    <span class="media-left">
                        <i class="icon fas fa-graduation-cap fa-fw " aria-hidden="true"></i>
                    </span>
                    <span class="media-body ">${ele.Course.CourseName}</span>
                </div>
                    </div >
                </a > `;
        $(`#nav-drawer #all-courses`).append(html);
    });
}

var getCourseSections = (id, token) =>
    $.ajax({
        url: api + `Courses(${id})?$expand=Sections`,
        type: "GET",
        headers: {
            "Authorization": `Bearer ${token}`,
        },
        success: (data) => loadSectionsToNav(data),
        error: () => alert("error"),
    });

const loadSectionsToNav = (data) => {
    let html = `<nav class="list-group">
        <a class="list-group-item list-group-item-action active" href="/Course/Details?id=${data.CourseId}" data-key="coursehome" data-isexpandable="0" data-indent="0" data-showdivider="0" data-type="60" data-nodetype="0" data-collapse="0" data-forceopen="1" data-isactive="1" data-hidden="0" data-preceedwithhr="0">
            <div class="m-l-0">
                <div class="media">
                    <span class="media-left">
                        <i class="icon fas fa-graduation-cap fa-fw " aria-hidden="true"></i>
                    </span>
                    <span class="media-body font-weight-bold">${data.CourseName}</span>
                </div>
            </div>
        </a>
        <a class="list-group-item list-group-item-action " href="" data-key="participants" data-isexpandable="0" data-indent="0" data-showdivider="0" data-type="90" data-nodetype="1" data-collapse="0" data-forceopen="0" data-isactive="0" data-hidden="0" data-preceedwithhr="0" data-parent-key="${data.CourseId}">
            <div class="m-l-0">
                <div class="media">
                    <span class="media-left">
                        <i class="icon fas fa-users fa-fw " aria-hidden="true"></i>
                    </span>
                    <span class="media-body ">Participants</span>
                </div>
            </div>
        </a>
        </nav>`;
    $(`#nav-drawer`).prepend(html);
    let i = 0;
    data["Sections"].forEach((ele) => {
        if ( "noseg" != ele.CourseName) {
            let html = `<a class="list-group-item list-group-item-action " href="#section-${i}" data-key="${ele.SectionId}" data-isexpandable="0" data-indent="0" data-showdivider="0" data-type="30" data-nodetype="1" data-collapse="0" data-forceopen="0" data-isactive="0" data-hidden="0" data-preceedwithhr="0" data-parent-key="${data.CourseId}">
        <div class="m-l-0">
        <div class="media">
        <span class="media-left">
        <i class="icon fas fa-folder fa-fw " aria-hidden="true"></i>
        </span>
        <span class="media-body ">${ele.SectionName}</span>
        </div>
        </div>
        </a>`;
            $(`#nav-drawer nav:first-of-type`).append(html);
            i++;
        }
    });
    
};