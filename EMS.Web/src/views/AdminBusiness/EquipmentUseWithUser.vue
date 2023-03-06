<template>
    <div>
        <div class="crumbs">
            <el-breadcrumb separator="/">
                <el-breadcrumb-item> <i class="el-icon-lx-cascades"></i> 使用申请 </el-breadcrumb-item>
            </el-breadcrumb>
        </div>
        <div class="container">
            <el-table
                :data="tableData"
                border
                class="table"
                ref="multipleTable"
                header-cell-class-name="table-header"
                @selection-change="handleSelectionChange"
            >
                <el-table-column type="selection" width="55" align="center"></el-table-column>
                <el-table-column prop="equipmentName" label="设备名称" align="center"></el-table-column>
                <el-table-column prop="useUserName" label="设备申请人" align="center"></el-table-column>
                <el-table-column prop="message" label="留言" align="center"></el-table-column>
                <el-table-column prop="returnSituationName" label="当前设备状态" align="center"></el-table-column>
                <el-table-column label="操作" width="180" align="center">
                    <template slot-scope="scope">
                        <el-button type="text" icon="el-icon-check" @click="handleEdit(scope.row)">通过申请</el-button>
                        <el-button type="text" icon="el-icon-delete" class="red" @click="handleDelete(scope.row)">不通过</el-button>
                    </template>
                </el-table-column>
            </el-table>
            <div class="pagination">
                <!-- <el-pagination
                    background
                    layout="total, prev, pager, next"
                    :current-page="query.pageIndex"
                    :page-size="query.pageSize"
                    :total="pageTotal"
                    @current-change="handlePageChange"
                ></el-pagination> -->
                <el-pagination
                        @current-change="handleCurrentChange"
                        :current-page.sync="currentPage"
                        :page-size="query.length"
                        layout="total, prev, pager, next"
                        :total="query.total"
                    >
                    </el-pagination>
            </div>
        </div>

        <!-- 编辑弹出框 -->
        <el-dialog title="操作" :visible.sync="editVisible" width="30%">
            <el-form ref="form" :model="form" label-width="120px">
                <el-form-item label="设备名称">
                    <el-input v-model="form.name"></el-input>
                </el-form-item>
                <el-form-item label="设备类型">
                    <el-select v-model="form.equipmentCategoryId" placeholder="请选择">
                        <el-option v-for="item in equipmentCategory" :key="item.id" :label="item.name" :value="item.id"> </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="设备购买日期">
                    <el-date-picker v-model="form.createTime" align="right" type="date" placeholder="选择日期"> </el-date-picker>
                </el-form-item>
                <!-- <el-form-item v-if="scrapShow">
                    <el-checkbox v-model="form.isScrap">是否报废</el-checkbox>
                </el-form-item> -->
            </el-form>
            <span slot="footer" class="dialog-footer">
                <el-button @click="editVisible = false">取 消</el-button>
                <el-button type="primary" @click="saveEdit">确 定</el-button>
            </span>
        </el-dialog>
    </div>
</template>

<script>
import { fetchData } from '../../api/index';
import api from '../../utils/request';
export default {
    name: 'Equipment',
    data() {
        return {
            equipmentCategory: [],
            query: {
                total:0,
                start:0,
                length:10
            },
            currentPage:1,
            tableData: [],
            multipleSelection: [],
            delList: [],
            editVisible: false,
            pageTotal: 0,
            form: {
                equipmentCategoryId: '',
                isScrap: false,
                name: '',
                createTime: null
            },
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
            let query = this.query;
            api.post('/EquipmentUseWithUser/getbysearch', {
                start: query.start,
                length: query.length
            }).then((x) => {
                this.tableData = x;
                for (var i = 0; i < x.length; i++) {
                    // this.tableData[i].createTime = x[i].createTime.substring(0, 19).replace('T', ' ');
                }
                console.log(x);
            });
            api.get('/EquipmentUseWithUser/GetBoCount').then((x) => {
                this.query.total = x;
            });
        },
        // 触发搜索按钮
        handleSearch() {
            this.$set(this.query, 'pageIndex', 1);
            this.getData();
            console.log(this.query);
        },
        // 删除操作
        handleDelete(row) {
            // 二次确认删除
            this.$confirm('确定不通过吗？', '提示', {
                type: 'warning'
            })
                .then(() => {
                    row.name = '未通过';
                    api.post('/EquipmentUseWithUser/update', row);
                })
                .catch(() => {});
        },
        // 多选操作
        handleSelectionChange(val) {
            this.multipleSelection = val;
        },
        delAllSelection() {
            let deleList = [];
            this.multipleSelection.forEach((x) => {
                deleList.push(x.id);
            });
            api.post('/Equipment/Deleterange', deleList).then((res) => {
                this.$message.error(res.message);
                this.getData();
            });
            this.multipleSelection = [];
        },
        // 编辑操作
        handleEdit(row) {
            row.name = '已通过';
            api.post('/EquipmentUseWithUser/update', row).then((x) => {
                this.$message.success('已通过');
                this.getData();
            });
        },
        addNew() {
            this.form = {};
            this.editVisible = true;
        },
        // 保存编辑
        saveEdit() {
            this.editVisible = false;
            console.log(this.form);
            api.post('/Equipment/update', this.form)
                .then((res) => {
                    this.$message.success(res.message);
                    this.getData();
                })
                .catch((err) => {
                    this.$message.error(rese.errMessage);
                });
            this.scrapShow = false;
            // this.$message.success(`修改第 ${this.idx + 1} 行成功`);
            // this.$set(this.tableData, this.idx, this.form);
        },
        // 分页导航
        handleCurrentChange(val) {
            // this.$set(this.query, 'pageIndex', val);
            // console.log(val)
            // this.query.pageStart  = (val - 1) * this.query.pageSize
            // console.log(this.query)
           
            this.query.start = this.query.length * (val - 1);
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
