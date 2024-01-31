import React, { useState, useRef } from "react";
import { DataTable, DataTableFilterMeta } from "primereact/datatable";
import { Column } from "primereact/column";
import { Button } from "primereact/button";
import { Toolbar } from "primereact/toolbar";
import { InputText } from "primereact/inputtext";
import RoleService from "../../services/role/role.services";
import { useQuery } from "@tanstack/react-query";
import UpsertRole from "./UpsertRole";
import { useToast } from "../../utils/toast";
import { confirmDialog } from "primereact/confirmdialog";
import { FilterMatchMode } from "primereact/api";
import { RoleDto } from "../../services/role/role.dto";
import { useTranslation } from "react-i18next";

const RolePage = () => {
  const [role, setRole] = useState<RoleDto>();
  const [filters, setFilters] = useState<DataTableFilterMeta>({
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
  });
  const [globalFilterValue, setGlobalFilterValue] = useState<string>("");
  const dt = useRef<DataTable<RoleDto[]>>(null);
  const [visibleUpsertRoleDialog, setVisibleUpsertRoleDialog] =
    useState<boolean>(false);
  const toast = useToast();
  const { t } = useTranslation();

  const { data: roleResponse, refetch: refetchRole } = useQuery({
    queryKey: ["Roles"],
    refetchOnMount: true,
    queryFn: () => RoleService.GetAll(),
  });

  const exportCSV = () => {
    dt.current?.exportCSV();
  };

  const leftToolbarTemplate = () => {
    return (
      <div className="flex flex-wrap gap-2">
        <Button
          label={t("NewRole")}
          icon="pi pi-plus"
          severity="success"
          onClick={() => {
            setVisibleUpsertRoleDialog(true);
          }}
        />
      </div>
    );
  };

  const rightToolbarTemplate = () => {
    return (
      <div className="flex flex-row gap-2">
        <Button
          type="button"
          icon="pi pi-file"
          rounded
          onClick={() => exportCSV()}
          data-pr-tooltip="CSV"
        />

        <Button
          type="button"
          icon="pi pi-file-excel"
          severity="success"
          rounded
          onClick={exportExcel}
          data-pr-tooltip="XLS"
        />
      </div>
    );
  };

  const actionBodyTemplate = (rowData: RoleDto) => {
    return (
      <>
        <Button
          icon="pi pi-pencil"
          rounded
          outlined
          className="mr-2"
          onClick={() => {
            setRole(rowData);
            setVisibleUpsertRoleDialog(true);
          }}
        />
        <Button
          icon="pi pi-trash"
          rounded
          outlined
          severity="danger"
          onClick={() => {
            deleteRoleConfirm(rowData);
          }}
        />
      </>
    );
  };

  const onGlobalFilterChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    let _filters = { ...filters };
    // @ts-ignore
    _filters["global"].value = value;
    setFilters(_filters);
    setGlobalFilterValue(value);
  };

  const header = (
    <div className="flex flex-wrap gap-2 align-items-center justify-content-between">
      <h4 className="m-0">{t("Roles")}</h4>
      <span className="p-input-icon-left">
        <i className="pi pi-search" />
        <InputText
          type="search"
          value={globalFilterValue}
          onChange={onGlobalFilterChange}
          placeholder={t("Search")}
        />
      </span>
    </div>
  );

  const deleteRole = (rowData: RoleDto) => {
    RoleService.Delete(rowData.id!).then((res) => {
      if (!res.error) {
        toast.show(t("RoleDeletedSuccessfully"), "success");
        refetchRole();
      }
    });
  };

  const deleteRoleConfirm = (rowData: RoleDto) => {
    confirmDialog({
      message: t("AreYouSureToDelete"),
      header: t("DeleteRole"),
      icon: "pi pi-info-circle",
      acceptClassName: "p-button-danger",
      accept: () => deleteRole(rowData),
      reject: () => {},
      acceptLabel: t("Yes"),
      rejectLabel: t("No"),
    });
  };

  // export excel
  const exportExcel = () => {
    import("xlsx").then((xlsx) => {
      const worksheet = xlsx.utils.json_to_sheet(
        roleResponse?.result?.data || []
      );
      const workbook = { Sheets: { data: worksheet }, SheetNames: ["data"] };
      const excelBuffer = xlsx.write(workbook, {
        bookType: "xlsx",
        type: "array",
      });

      saveAsExcelFile(excelBuffer, "roles");
    });
  };

  const saveAsExcelFile = (buffer: any, fileName: any) => {
    import("file-saver").then((module) => {
      if (module && module.default) {
        let EXCEL_TYPE =
          "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8";
        let EXCEL_EXTENSION = ".xlsx";
        const data = new Blob([buffer], {
          type: EXCEL_TYPE,
        });

        module.default.saveAs(
          data,
          fileName + "_export_" + new Date().getTime() + EXCEL_EXTENSION
        );
      }
    });
  };

  return (
    <div>
      <div className="card">
        <Toolbar
          className="mb-4"
          start={leftToolbarTemplate}
          end={rightToolbarTemplate}
        ></Toolbar>

        <DataTable
          ref={dt}
          value={roleResponse?.result?.data || []}
          dataKey="id"
          paginator
          rows={10}
          rowsPerPageOptions={[10, 25, 50, 100]}
          paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
          currentPageReportTemplate="Total {totalRecords} Roles"
          filters={filters}
          // filterDisplay="row"
          loading={roleResponse?.isLoading}
          globalFilterFields={["name"]}
          emptyMessage={t("NoRecordsFound")}
          header={header}
        >
          <Column field="name" header={t("Name")} sortable></Column>
          <Column
            field="permissions"
            header={t("Permissions")}
            sortable
            body={(rowData: RoleDto) => {
              return (
                <span>
                  {rowData.permissions
                    .map((permission) => permission.name)
                    .join(", ")}
                </span>
              );
            }}
          ></Column>
          <Column
            body={actionBodyTemplate}
            exportable={false}
            className="flex justify-content-center"
            style={{ minWidth: "9rem" }}
          ></Column>
        </DataTable>
      </div>

      <UpsertRole
        visible={visibleUpsertRoleDialog}
        setVisible={setVisibleUpsertRoleDialog}
        role={role}
        setRole={setRole}
        refetchRole={refetchRole}
      />
    </div>
  );
};

export default RolePage;
