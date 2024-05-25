$(document).ready(() => {
    WireEventsToElements();

    GetProjectsAsync();
});

function WireEventsToElements() {
    // edit Project modal dialog
    $(() => {
        $("#divEditProjectDialog").show();

        $("#divEditProjectDialog").dialog({
            autoOpen: false,
            title: 'Edit Project',
            maxWidth: 600,
            maxHeight: 500,
            width: 600,
            height: 400
        });
    });

    // StartDate date picker
    $(() => {
        $("#dpEditProjectStartDate").datepicker();
    });

    // EndDate date picker
    $(() => {
        $("#dpEditProjectEndDate").datepicker();
    });

    // New record link click event
    $('a.editor-create').on('click', function (e) {
        e.preventDefault();

        $('#divEditProjectDialog').dialog('open');

        $("#txtProjectName").blur();
    });

    // Edit record icon click event
    $('#tblProjects').on('click', 'td.editor-edit', function (e) {
        let $row = $(this).closest('tr');
        let data = $('#tblProjects').DataTable().row($row).data();

        e.preventDefault();

        $("#hdnProjectID").val(data['projectID']);
        $("#txtProjectName").val(data['projectName']);
        $("#dpEditProjectStartDate").val(new Date(data['startDate']).toLocaleDateString("en-US"));
        $("#dpEditProjectEndDate").val(new Date(data['endDate']).toLocaleDateString("en-US"));

        $('#divEditProjectDialog').dialog('open');

        $("#txtProjectName").blur();
    });

    // Delete a record icon click event
    $('#tblProjects').on('click', 'td.editor-delete', function (e) {
        let $row = $(this).closest('tr');
        let data = $('#tblProjects').DataTable().row($row).data();
        let TblProject = {
            ProjectID: data['projectID'],
            ProjectName: data['projectName'],
            StartDate: new Date(data['startDate']),
            EndDate: new Date(data['endDate'])
        };

        e.preventDefault();

        swal({
            title: "Are you sure?",
            text: "Delete Project",
            icon: "warning",
            buttons: [
                'Yes',
                'No'
            ],
            dangerMode: true,
        }).then(function (isConfirm) {
            if (!isConfirm) {
                ShowSpinnerHideTable();

                $('#tblProjects').DataTable().destroy();

                DeleteProjectAsync(TblProject);
            } else {
                swal("Cancelled", "Project kept", "error");
            }
        });
    });
}

async function GetProjectsAsync() {
    let urlGetProjects = $("#divIndex").data("getProjectsUrl");

    try {
        const settingsGetProjectsAsyncGet = {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            global: true,
            dataType: 'json'
        };
        const responseGetProjectsAsyncGet = await fetch(urlGetProjects, settingsGetProjectsAsyncGet);
        let results;

        // invalid response
        if (!responseGetProjectsAsyncGet.ok) {
            toastr.error("Error occured");

            return;
        }

        results = await responseGetProjectsAsyncGet.json();

        BuildDatatable(results);
    }
    catch (err) {
        toastr.error("Error occured");

        console.log(err);
    }
}

function BuildDatatable(dataProjects) {
    $('#tblProjects').DataTable({
        data: dataProjects,
        columns: [
            { "data": "projectID", className: "dt-body-left" },
            { "data": "projectName", className: "dt-body-left" },
            { "data": "startDate", className: "dt-body-left" },
            { "data": "endDate", className: "dt-body-left" },
            {
                data: null,
                className: "dt-body-center editor-edit",
                defaultContent: '<i class="fa fa-pencil"></i>',
                orderable: false
            },
            {
                data: null,
                className: "dt-body-center editor-delete",
                defaultContent: '<i class="fa fa-trash"></i>',
                orderable: false
            }
        ],
        order: [[1, "asc"]],
        columnDefs: [
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [2],
                "render": DataTable.render.moment('YYYY-MM-DDTHH:mm:ss', 'MM/DD/YYYY')
            },
            {
                "targets": [3],
                "render": DataTable.render.moment('YYYY-MM-DDTHH:mm:ss', 'MM/DD/YYYY')
            }
        ],
        "bStateSave": true,
        "fnStateSave": function (oSettings, oData) {
            localStorage.setItem('tblProjects', JSON.stringify(oData));
        },
        "fnStateLoad": function (oSettings) {
            return JSON.parse(localStorage.getItem('tblProjects'));
        }
    });

    HideSpinnerShowTable();
}

function SaveProjectEditModal() {
    ShowSpinnerHideTable();

    $('#tblProjects').DataTable().destroy();

    SaveProjectAsync();
}

async function SaveProjectAsync() {
    let urlPostOrPatchProject = $("#divIndex").data("postOrPatchProjectUrl");
    let startDateParse = Date.parse($("#dpEditProjectStartDate").val());
    let endDateParse = Date.parse($("#dpEditProjectEndDate").val());
    let startDate = new Date(startDateParse);
    let endDate = new Date(endDateParse);
    let projectID = $("#hdnProjectID").val() != "" ? $("#hdnProjectID").val() : "";
    let TblProject = {
        ProjectID: projectID,
        ProjectName: $("#txtProjectName").val(),
        StartDate: startDate,
        EndDate: endDate
    };
    let restMethod = TblProject.ProjectID != "" ? "PATCH" : "POST";

    try {
        const settingsSaveProjectAsyncPatch = {
            method: restMethod,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            global: true,
            dataType: 'json',
            body: JSON.stringify(TblProject)
        };
        const responseSaveProjectAsyncPatch = await fetch(urlPostOrPatchProject, settingsSaveProjectAsyncPatch);

        // invalid response
        if (!responseSaveProjectAsyncPatch.ok) {
            toastr.error("Error occured");
        }
        else {
            if (projectID != "") {
                toastr.success("Project updated");
            }
            else {
                toastr.success("Project created");
            }
        }

        GetProjectsAsync();

        CloseProjectEditModal();
    }
    catch (err) {
        toastr.error("Error occured");

        console.log(err);
    }
}

async function DeleteProjectAsync(TblProject) {
    let urlPatchProject = $("#divIndex").data("deleteProjectUrl");

    try {
        const settingsDeleteProjectAsyncDelete = {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            global: true,
            dataType: 'json',
            body: JSON.stringify(TblProject)
        };
        const responseDeleteProjectAsyncDelete = await fetch(urlPatchProject, settingsDeleteProjectAsyncDelete);

        // invalid response
        if (!responseDeleteProjectAsyncDelete.ok) {
            toastr.error("Error occured");
        }
        else {
            toastr.success("Project deleted");
        }

        GetProjectsAsync();
    }
    catch (err) {
        toastr.error("Error occured");

        console.log(err);
    }
}

function CloseProjectEditModal() {
    $("#hdnProjectID").val("");
    $("#txtProjectName").val("");
    $("#dpEditProjectStartDate").val("");
    $("#dpEditProjectEndDate").val("");

    $('#divEditProjectDialog').dialog('close');
}

function HideSpinnerShowTable() {
    $("#divSpinnerLoading").hide();
    $("#divCreateProject").show();
    $("#tblProjects").show();
}

function ShowSpinnerHideTable() {
    $("#divSpinnerLoading").show();
    $("#divCreateProject").hide();
    $("#tblProjects").hide();
}
