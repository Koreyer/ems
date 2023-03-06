<template>
    <div>
        <div class="crumbs">
            <el-breadcrumb separator="/">
                <el-breadcrumb-item> <i class="el-icon-lx-cascades"></i> 维修申请表 </el-breadcrumb-item>
            </el-breadcrumb>
        </div>
        <div class="container">
            <div class="handle-box">
                <el-input v-model="query.value" placeholder="用户名" class="handle-input mr10"></el-input>
                <el-button type="primary" icon="el-icon-search" @click="handleSearch">搜索</el-button>
            </div>
            <el-table
                :data="tableData"
                border
                class="table"
                ref="multipleTable"
                header-cell-class-name="table-header"
                @selection-change="handleSelectionChange"
            >
                <el-table-column type="selection" width="55" align="center"></el-table-column>
                <el-table-column prop="equipmentName" label="设备名称"></el-table-column>
                <el-table-column prop="reportUserName" label="报修人员"></el-table-column>
                <el-table-column prop="createTime" label="报修时间"></el-table-column>
                <el-table-column prop="message" label="留言"></el-table-column>
                <el-table-column label="操作" width="180" align="center">
                    <template slot-scope="scope">
                        <el-button type="text" icon="el-icon-check" @click="handleEdit(scope.row)">维修完成</el-button>
                        <el-button type="text" icon="el-icon-delete" class="red" @click="handleDelete(scope.row)">报废</el-button>
                    </template>
                </el-table-column>
            </el-table>
            <div class="pagination">
                <el-pagination
                    background
                    layout="total, prev, pager, next"
                    :current-page="query.pageIndex"
                    :page-size="query.pageSize"
                    :total="pageTotal"
                    @current-change="handlePageChange"
                ></el-pagination>
            </div>
        </div>
    </div>
</template>

<script>
import api from '../../utils/request';
import { fetchData } from '../../api/index';
export default {
    name: 'equipmentreport',
    data() {
        return {
            query: {
                value: '',
                name: '',
                start: 0,
                length: 10
            },
            tableData: [],
            multipleSelection: [],
            delList: [],
            editVisible: false,
            pageTotal: 0,
            form: {},
            idx: -1,
            id: -1
        };
    },
    created() {
        this.getData();
    },
    methods: {
        // 获取 easy-mock 的模拟数据
        getData() {
            api.post('/EquipmentReport/getbysearch', {
                start: this.query.start,
                length: this.query.length,
                searchValue: this.query.value
            }).then((res) => {
                this.tableData = res;
                for (var i = 0; i < res.length; i++) {
                    this.tableData[i].createTime = res[i].createTime.substring(0, 19).replace('T', ' ');
                }
            });
            api.get('/EquipmentReport/GetBoCount',{}).then(x=>{
                this.pageTotal = x
            })
        },
        // 触发搜索按钮
        handleSearch() {
            this.getData();
        },
        // 删除操作
        handleDelete(row) {
            // 二次确认删除
            this.$confirm('确定要报废吗？', '提示', {
                type: 'warning'
            })
                .then(() => {
                    row.isRepair = true;
                    row.createTime = row.createTime.replace(' ', 'T');
                    row.repairUserId = localStorage.getItem('acc_id')
                    //报修添加为已维修
                    api.post('/EquipmentReport/Update', row).then((x) => {
                        //添加设备进报废
                        api.post('/EquipmentScrap/Update', { equipmentId: row.equipmentId }).then((res) => {
                            this.$message.error('报废成功');
                        });
                    });
                    // api.post('/EquipmentScrap/test', { equipmentId: row.equipmentId }).then((res) => {
                    // // 更新设备为报废设备
                    // api.post('/Equipment/Update', { id: row.equipmentId, isScrap: true }).then((r) => {
                    //     this.$message.success('报废成功');
                    // });
                    // });
                })
                .catch(() => {});
        },
        // 多选操作
        handleSelectionChange(val) {
            this.multipleSelection = val;
        },
        delAllSelection() {
            const length = this.multipleSelection.length;
            let str = '';
            this.delList = this.delList.concat(this.multipleSelection);
            for (let i = 0; i < length; i++) {
                str += this.multipleSelection[i].name + ' ';
            }
            this.$message.error(`删除了${str}`);
            this.multipleSelection = [];
        },
        // 编辑操作
        handleEdit(row) {
            row.isRepair = true;
            this.$confirm('确定要报废吗？', '提示', {
                type: 'warning'
            }).then(() => {
                api.post('/EquipmentReport/Update', row).then((x) => {
                    this.$message.success('维修成功');
                });
            });
        },
        // 保存编辑
        saveEdit() {
            this.editVisible = false;
            this.$message.success(`修改第 ${this.idx + 1} 行成功`);
            this.$set(this.tableData, this.idx, this.form);
        },
        // 分页导航
        handlePageChange(val) {
            this.$set(this.query, 'pageIndex', val);
            this.getData();
        }
    }
};
</script>

<style scoped>
.handle-box {
    margin-bottom: 20px;
}

.handle-select {
    width: 120px;
}

.handle-input {
    width: 300px;
    display: inline-block;
}
.table {
    width: 100%;
    font-size: 14px;
}
.red {
    color: #ff0000;
}
.mr10 {
    margin-right: 10px;
}
.table-td-thumb {
    display: block;
    margin: auto;
    width: 40px;
    height: 40px;
}
</style>
