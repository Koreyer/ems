<template>
    <div>
        <div class="crumbs">
            <el-breadcrumb separator="/">
                <el-breadcrumb-item> <i class="el-icon-lx-cascades"></i> 事务通报 </el-breadcrumb-item>
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
                <el-table-column prop="name" label="标题" align="center"></el-table-column>
                <el-table-column label="通报人" prop="userName" align="center"> </el-table-column>
                <el-table-column prop="createTime" label="修改时间" align="center"></el-table-column>
                <el-table-column label="内容" width="200">
                    <template slot-scope="scope">
                        <el-input placeholder="请输入内容" :readonly="true" type="textarea" v-model="scope.row.content"> </el-input>
                    </template>
                </el-table-column>
                <el-table-column label="操作" width="180" align="center">
                    <template slot-scope="scope">
                        <el-button type="text" icon="el-icon-edit" @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
                        <el-button type="text" icon="el-icon-delete" class="red" @click="handleDelete(scope.$index, scope.row)"
                            >删除</el-button
                        >
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
        <el-dialog title="编辑" :visible.sync="editVisible" width="30%">
            <el-form ref="form" :model="form" label-width="70px">
                <el-form-item label="标题">
                    <el-input v-model="form.name"></el-input>
                </el-form-item>
                <el-form-item label="内容">
                    <el-input v-model="form.content" type="textarea"></el-input>
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
    name: 'transactionnotification',
    data() {
        return {
            query: {
                searchValue: '',
                pageIndex: 1,
                pageSize: 4
            },
            tableData: [],
            multipleSelection: [],
            delList: [],
            editVisible: false,
            pageTotal: 0,
            form: {},
            accid: localStorage.getItem('acc_id'),
            accname: localStorage.getItem('ms_username'),
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
            api.post('/TransactionNotification/getbysearch', {
                start: (query.pageIndex - 1) * query.pageSize,
                length: query.pageSize,
                searchValue: query.searchValue
            }).then((x) => {
                this.tableData = x;
                for (var i = 0; i < x.length; i++) {
                    this.tableData[i].createTime = x[i].createTime.substring(0, 19).replace('T', ' ');
                }
                console.log(x);
            });

            api.get('/TransactionNotification/GetBoCount').then((res) => {
                this.pageTotal = res;
            });
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
                    api.del('/TransactionNotification/Delete?id=' + row.id)
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
            api.post('/TransactionNotification/Deletes', deleList).then((res) => {
                this.$message.error(res.message);
                this.getData();
            });
            this.multipleSelection = [];
        },
        // 编辑操作
        handleEdit(index, row) {
            this.idx = index;
            this.form = row;
            this.form.createTime = row.createTime.replace(' ', 'T');
            this.editVisible = true;
        },
        addNew() {
            this.form = {
                title: '',
                name: '',
                userId: this.accid,
                content: '',
                userName: this.accname
            };
            this.editVisible = true;
        },
        // 保存编辑
        saveEdit() {
            this.editVisible = false;
            api.post('/TransactionNotification/update', this.form)
                .then((res) => {
                    this.$message.success(res.message);
                    this.getData();
                })
                .catch((err) => {
                    this.$message.error(rese.errMessage);
                });
            // this.$message.success(`修改第 ${this.idx + 1} 行成功`);
            // this.$set(this.tableData, this.idx, this.form);
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
