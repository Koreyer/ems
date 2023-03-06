<template>
    <div>
        <div class="crumbs">
            <el-breadcrumb separator="/">
                <el-breadcrumb-item> <i class="el-icon-lx-cascades"></i> 设备信息 </el-breadcrumb-item>
            </el-breadcrumb>
        </div>
        <div class="container">
            <div class="handle-box">
                <el-button type="primary" icon="el-icon-delete" class="handle-del mr10" @click="delAllSelection">批量删除</el-button>
                <el-input v-model="query.searchValue" placeholder="标题" class="handle-input mr10"></el-input>
                <el-button type="primary" icon="el-icon-search" @click="handleSearch">搜索</el-button>
                <el-button type="primary" icon="el-icon-search" @click="addNew">新增</el-button>
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
                <el-table-column prop="name" label="设备名称" align="center"></el-table-column>
                <el-table-column prop="createTime" label="购买日期" align="center"></el-table-column>
                <el-table-column prop="equipmentCategoryName" label="设备类型" align="center"></el-table-column>
                <el-table-column label="是否借出" width="180" align="center">
                    <template slot-scope="scope">
                        <span v-if="scope.row.islend">是</span>
                        <span v-else>否</span>
                    </template>
                </el-table-column>
                <el-table-column label="操作" width="180" align="center">
                    <template slot-scope="scope">
                        <el-button type="text" icon="el-icon-edit" @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
                        <!-- <el-button type="text" icon="el-icon-delete" class="red" @click="handleDelete(scope.$index, scope.row)"
                            >删除</el-button
                        > -->
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
                    <el-date-picker value-format="yyyy-MM-dd" v-model="form.createTime" type="date" placeholder="选择日期">
                    </el-date-picker>
                </el-form-item>
                <el-form-item v-if="scrapShow">
                    <el-checkbox v-model="form.isScrap">是否报废</el-checkbox>
                </el-form-item>
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
                searchValue: '',
                pageIndex: 1,
                pageSize: 10
            },
            tableData: [],
            multipleSelection: [],
            delList: [],
            editVisible: false,
            pageTotal: 0,
            scrapShow: false,
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
            api.post('/Equipment/getbysearch', {
                start: (query.pageIndex - 1) * query.pageSize,
                length: query.pageSize,
                searchValue: query.searchValue
            }).then((x) => {
                this.tableData = x;
                for (var i = 0; i < x.length; i++) {
                    this.tableData[i].createTime = x[i].createTime.substring(0, 10);
                }
                console.log(x);
            });
            api.post('/EquipmentCategory/getbysearch', {}).then((x) => {
                this.equipmentCategory = x;
                console.log(this.equipmentCategory);
            });
            api.get('/Equipment/GetBoCount',{}).then(x=>{
                this.pageTotal = x
            })
        },
        // 触发搜索按钮
        handleSearch() {
            this.$set(this.query, 'pageIndex', 1);
            this.getData();
            console.log(this.query);
        },
        // 删除操作
        handleDelete(index, row) {
            // 二次确认删除
            this.$confirm('确定要删除吗？', '提示', {
                type: 'warning'
            })
                .then(() => {
                    api.del('/Equipment/Delete?id=' + row.id)
                        .then((res) => {
                            if (res.message == '删除成功') {
                                this.$message.success('删除成功');
                                this.tableData.splice(index, 1);
                            }
                        })
                        .catch((err) => {
                            console.log(err);
                        });
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
        handleEdit(index, row) {
            this.idx = index;
            this.form = row;
            this.scrapShow = true;
            this.editVisible = true;
        },
        addNew() {
            this.form = {};
            this.editVisible = true;
            this.scrapShow = false;
        },
        // 保存编辑
        saveEdit() {
            let form = this.form
            console.log(form)
            if(form.name == "undefined" || form.equipmentCategoryId == undefined || form.createTime == undefined)
            {
                this.$message.error("请输入完整信息");
            }else{
                this.editVisible = false;
            console.log(this.form);
            api.post('/Equipment/update', this.form)
                .then((res) => {
                    this.$message.success(res.message);
                    console.log(this.form)
                    if(this.form.isScrap == true)
                    api.post('/EquipmentScrap/Update', { equipmentId: this.form.id }).then((res) => {
                            this.$message.error('报废成功');
                        });
                    this.getData();
                })
                .catch((err) => {
                    this.$message.error(rese.errMessage);
                });
            this.scrapShow = false;
            }

            

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
