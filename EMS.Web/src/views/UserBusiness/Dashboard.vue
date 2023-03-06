<template>
    <div >
        <el-row :gutter="5">
            <el-col :span="24">
                <el-card shadow="hover" class="mgb20" style="height: 122px width:100%">
                    <div class="user-info">
                        <img src="../../assets/img/img.jpg" class="user-avator" alt />
                        <el-dropdown class="user-name" trigger="click" @command="handleCommand">
                            <span class="el-dropdown-link">
                                <div class="user-info-cont">
                                    <div class="user-info-name">
                                        {{ name }}
                                        <i class="el-icon-caret-bottom"></i>
                                    </div>

                                    <div>学生</div>
                                </div>
                            </span>
                            <el-dropdown-menu slot="dropdown">
                                <el-dropdown-item divided command="passShow">修改密码</el-dropdown-item>
                                <el-dropdown-item divided command="loginout">退出登录</el-dropdown-item>
                            </el-dropdown-menu>
                        </el-dropdown>
                    </div>
                </el-card>
            </el-col>
            
        </el-row>
        <el-row :gutter="18">
            <el-col :span="18">
                <el-card shadow="hover" style="height: 355px">
                    <div slot="header" class="clearfix">
                        <span>申请使用设备</span>
                        <el-button style="float: right" type="primary" @click="editVisible = true">申请使用</el-button>
                    </div>
                    <el-table :data="withData" border class="table" max-height="250">
                        <el-table-column width="auto" align="center" prop="equipmentName" label="设备名称"></el-table-column>
                        <el-table-column width="auto" align="center" prop="examineEnumName" label="审核状态"></el-table-column>
                        <el-table-column width="auto" align="center" prop="returnTime" label="归还时间"></el-table-column>
                        <el-table-column width="auto" align="center" label="是否归还">
                            <template slot-scope="scope">
                                <span v-if="scope.row.isReturn">是</span>
                                <span v-else>否</span>
                            </template>
                        </el-table-column>
                        <el-table-column width="150rpx"  align="center"  label="留言">
                            <template slot-scope="scope">
                        <el-input placeholder="请输入内容" :readonly="true" type="textarea" v-model="scope.row.message"> </el-input>
                    </template>
                        </el-table-column>
                        <el-table-column label="操作" width="180" align="center">
                            <template slot-scope="scope">
                                <el-button
                                    v-if="scope.row.name == '已通过' && scope.row.isReturn == false"
                                    type="text"
                                    icon="el-icon-edit"
                                    @click="handleEdit(scope.row)"
                                    >归还设备</el-button
                                >
                            </template>
                        </el-table-column>
                    </el-table>
                    <el-pagination
                        @current-change="handleCurrentChange"
                        :current-page.sync="currentPage"
                        :page-size="5"
                        layout="total, prev, pager, next"
                        :total="query.total"
                    >
                    </el-pagination>
                </el-card>
            </el-col>
            <el-col :span="6">
                <el-card shadow="hover" style="height: 355px">
                    <div slot="header" class="clearfix">
                        <span>事务通报</span>
                    </div>
                    <el-table :show-header="false" :data="todoList" style="width: 100%">
                        <el-table-column>
                            <template slot-scope="scope">
                                <div class="todo-item" @click="todo(scope.row)">{{ scope.row.title }}</div>
                            </template>
                        </el-table-column>
                    </el-table>
                    <el-pagination
                        @current-change="todoChange"
                        :current-page.sync="todoCurrentPage"
                        :page-size="6"
                        layout="total, prev, pager, next"
                        :total="todoQuery.total"
                    >
                    </el-pagination>
                </el-card>
            </el-col>
        </el-row>

        <el-dialog title="申请使用设备" :visible.sync="editVisible" width="40%">
            <el-form ref="form" :model="form" label-width="120px">
                <el-form-item label="申请的设备">
                    <div class="block">
                        <el-cascader
                            v-model="equipmentCategoryId"
                            :options="equipmentList"
                            :props="{ expandTrigger: 'hover' }"
                            @change="handleChange"
                            :show-all-levels="false"
                        ></el-cascader>
                    </div>
                </el-form-item>
                <el-form-item label="留言">
                    <el-input v-model="form.message" type="textarea"></el-input>
                </el-form-item>
            </el-form>
            <span slot="footer" class="dialog-footer">
                <el-button @click="editVisible = false">取 消</el-button>
                <el-button type="primary" @click="saveEdit">确 定</el-button>
            </span>
        </el-dialog>
        <el-dialog title="修改密码" :visible.sync="passShow" width="40%">
            <el-form ref="user" :model="user" label-width="120px">
                <el-form-item label="输入密码">
                    <el-input v-model="user.password"></el-input>
                </el-form-item>
                <el-form-item label="确认密码">
                    <el-input v-model="user.passwordAge"></el-input>
                </el-form-item>
            </el-form>
            <span slot="footer" class="dialog-footer">
                <el-button @click="passShow = false">取 消</el-button>
                <el-button type="primary" @click="passEdit">确 定</el-button>
            </span>
        </el-dialog>
        <el-dialog title="归还设备" :visible.sync="returnShow" width="40%">
            <el-form ref="form" :model="form" label-width="120px">
                <el-form-item label="当前设备状态">
                    <el-select
                        v-model="form.returnSituationName"
                        class="m-2"
                        placeholder="请选择设备状态"
                        size="large"
                        @change="selectChange"
                    >
                        <el-option v-for="item in situationList" :key="item" :label="item" :value="item" />
                    </el-select>
                </el-form-item>
                <el-form-item v-if="reportCheck" label="是否报修">
                    <el-checkbox v-model="reportCheck" :disabled="true">报修</el-checkbox>
                </el-form-item>
                <el-form-item v-if="reportCheck" label="留言">
                    <el-input v-model="message"></el-input>
                </el-form-item>
            </el-form>
            <span slot="footer" class="dialog-footer">
                <el-button @click="returnClose">取 消</el-button>
                <el-button type="primary" @click="returnEdit">确 定</el-button>
            </span>
        </el-dialog>
        <el-dialog title="事务通报" :visible.sync="todoShow" width="80%">
            <h3>标题：{{ trans.title }}</h3>
            <br />
            <span>通报人：{{ trans.userName }}</span
            ><br /><br />
            <span>内容：{{ trans.content }}</span>
            <span slot="footer" class="dialog-footer">
                <el-button type="primary" @click="todoShow = false">确 认</el-button>
            </span>
        </el-dialog>
        <el-col :span="16">
            <el-card shadow="hover" style="height: 403px">
                <div slot="header" class="clearfix">
                    <span>报修列表</span>
                    <!-- <el-button style="float: right" type="primary" @click="editVisible = true">申请报修</el-button> -->
                </div>
                <el-table :data="equipmentReport" border class="table" max-height="250">
                    <el-table-column width="auto" align="center" prop="equipmentName" label="设备名称"></el-table-column>
                    <el-table-column width="auto" align="center" prop="repairUserName" label="维修人员"></el-table-column>
                    <el-table-column width="auto" align="center" prop="createTime" label="报修时间"></el-table-column>
                    <el-table-column width="auto" align="center"  label="维修时间">
                        <template slot-scope="scope">
                            <span v-if="scope.row.updateTime != null">{{scope.row.updateTime.substring(0,19).replace("T"," ")}}</span>
                            <span v-else>{{scope.row.updateTime}}</span>
                        </template>
                    </el-table-column>
                    <el-table-column width="100rpx" align="center" label="是否维修">
                        <template slot-scope="scope">
                            <span v-if="scope.row.isRepair">是</span>
                            <span v-else>否</span>
                        </template>
                    </el-table-column>
                    <el-table-column width="450rpx" align="center" prop="message" label="留言"></el-table-column>
                </el-table>
                <el-pagination
                    @current-change="handleCurrentChange"
                    :current-page.sync="currentPage"
                    :page-size="5"
                    layout="total, prev, pager, next"
                    :total="reportQuery.total"
                >
                </el-pagination>
            </el-card>
        </el-col>
        <el-col :span="8">
            <el-card shadow="hover" style="height: 403px">
                <div slot="header" class="clearfix">
                    <span>可用设备列表</span>
                    <el-input v-model="equipmenQuery.value" clearable placeholder="设备名" class="handle-input mr10"></el-input>
                    <el-button type="primary" icon="el-icon-search" @click="handleSearch">搜索</el-button>
                </div>
                <el-table :data="equipmentData" border class="table" max-height="250">
                    <el-table-column width="auto" align="center" prop="name" label="设备名称"></el-table-column>
                    <el-table-column width="auto" align="center" prop="equipmentCategoryName" label="设备类别"></el-table-column>
                </el-table>
                <el-pagination @current-change="equChange" :page-size="5" layout="total, prev, pager, next" :total="equipmenQuery.total">
                </el-pagination>
            </el-card>
        </el-col>
    </div>
</template>

<script>
import api from '../../utils/request';
import Schart from 'vue-schart';
import bus from '../AdminBusiness/common/bus';
export default {
    name: 'dashboard',
    data() {
        return {
            name: localStorage.getItem('ms_username'),
            todoList: [],
            withData: [],
            currentPage: 1,
            todoCurrentPage: 1,
            query: {
                start: 0,
                length: 5,
                total: 0
            },
            todoQuery: {
                start: 0,
                length: 5,
                total: 0
            },
            reportQuery: {
                start: 0,
                length: 5,
                total: 0
            },
            equipmenQuery: {
                start: 0,
                length: 5,
                total: 0,
                value: ''
            },
            user: {},
            passShow: false,
            situationList: ['设备良好', '设备异常'],
            equipmentList: [],
            equipmentData: [],
            equipmentCategoryId: '',
            editVisible: false,
            returnShow: false,
            todoShow: false,
            trans: [],
            form: {
                equipmentId:"",
                message:""
            },
            message: '',
            reportCheck: false,
            accid: localStorage.getItem('acc_id'),
            equipmentReport: []
        };
    },
    created() {
        this.getData();
        console.log(this.accid);
    },
    components: {
        Schart
    },
    computed: {},
    methods: {
        //获取设备列表
        getequipmentData() {
            api.post('/equipment/get', {
                start: this.equipmenQuery.start,
                length: this.equipmenQuery.length,
                searchValue: this.equipmenQuery.value
            }).then((res) => {
                this.equipmentData = res;
            });
        },
        //获取设备总数
        getEquipmentCount() {
            api.get('/equipment/GetCount').then((x) => {
                this.equipmenQuery.total = x;
            });
        },

        //获取申请使用记录
        getEquipmentUseWithUser() {
            api.get('/EquipmentUseWithUser/Get?Id=' + this.accid + '&start=' + this.query.start + '&length=' + this.query.length).then(
                (res) => {
                    this.withData = res;
                    res.forEach((item, key) => {
                        if (typeof item.returnTime == typeof '')
                            this.withData[key].returnTime = item.returnTime.substring(0, 19).replace('T', ' ');
                    });
                }
            );
        },
        //获取申请使用总数
        getEquipmentUseWithUserCount() {
            api.get('/EquipmentUseWithUser/GetBoCountById?id='+this.accid,{}).then((res) => {
                this.query.total = res;
                console.log(this.accid);
            });
        },
        //获取事务通报
        getTransactionNotification() {
            api.get('/TransactionNotification/Get?start=' + this.todoQuery.start + '&length=' + this.todoQuery.length).then((res) => {
                this.todoList = res;
                console.log(this.todoList);
            });
        },
        //获取事务通报数量
        getTransactionNotificationCount() {
            api.get('/TransactionNotification/GetBoCount').then((res) => {
                this.todoQuery.total = res;
                console.log(res);
            });
        },
        //自己报修的设备
        getequipmentReport() {
            api.get(
                '/equipmentReport/getbyid?id=' + this.accid + '&start=' + this.reportQuery.start + '&length=' + this.reportQuery.length
            ).then((res) => {
                this.equipmentReport = res;
                res.forEach((item, key) => {
                    if (typeof item.createTime == typeof '')
                        this.equipmentReport[key].createTime = item.createTime.substring(0, 19).replace('T', ' ');
                    if (item.updateTime == '0001-01-01T00:00:00') this.equipmentReport[key].updateTime = null;
                });
                this.reportQuery.total = res.length;
            });
        },
        getData() {
            this.getequipmentData();
            this.getEquipmentUseWithUser();
            this.getEquipmentUseWithUserCount();
            this.getTransactionNotification();
            this.getTransactionNotificationCount();
            this.getequipmentReport();
            this.getEquipmentCount();
            let equipmen = api.post('/Equipment/getbysearch', { start: 0, length: 100 });
            let equipmentCategory = api.post('/EquipmentCategory/getbysearch', {});
            equipmentCategory.then((res) => {
                equipmen.then((re) => {
                    res.forEach((item, key) => {
                        this.equipmentList.push({
                            value: item.id,
                            label: item.name,
                            children: []
                        });
                        re.forEach((x) => {
                            if (x.equipmentCategoryId == item.id) {
                                this.equipmentList[key].children.push({
                                    value: x.id,
                                    label: x.name
                                });
                            }
                        });
                    });
                });
            });
        },
        changeDate() {
            const now = new Date().getTime();
            this.data.forEach((item, index) => {
                const date = new Date(now - (6 - index) * 86400000);
                item.name = `${date.getFullYear()}/${date.getMonth() + 1}/${date.getDate()}`;
            });
        },
        handleChange() {},
        saveEdit() {
           
            this.form.equipmentId = this.equipmentCategoryId[1];
            if(this.form.equipmentId == undefined || this.form.message == ""){
                this.$message.error("请填写完整");
            }else{
this.editVisible = false;
            this.form.useUserId = this.accid;
            api.post('/EquipmentUseWithUser/update', this.form)
                .then((res) => {
                    this.$message.success(res.message);
                    this.form = {};
                    this.equipmentList = [];
                    this.getData();
                })
                .catch((err) => {
                    console.log(err);
                });
            }
            
        },
        todo(row) {
            this.todoShow = true;
            this.trans = row;
        },
        handleCommand(command) {
            if (command == 'loginout') {
                // localStorage.removeItem('ms_username');
                localStorage.clear();
                this.$router.push('/login');
            }
            if (command == 'passShow') {
                this.passShow = true;
            }
        },
        returnClose() {
            this.returnShow = false;
        },
        handleEdit(row) {
            this.returnShow = true;
            this.form = row;
        },
        returnEdit() {
            this.returnShow = false;
            this.form.isReturn = true;
            if (this.form.returnSituationName != '设备良好') {
                api.post('/EquipmentReport/Update', {
                    reportUserId: this.accid,
                    equipmentId: this.form.equipmentId,
                    message: this.message
                });
            }
            api.post('/EquipmentUseWithUser/update', this.form).then((res) => {
                this.$message.success('归还成功');
                this.getData();
            });
        },
        handleCurrentChange(val) {
            this.query.start = this.query.length * (val - 1);
            this.getEquipmentUseWithUser();
        },
        equChange(val) {
            this.equipmenQuery.start = this.equipmenQuery.length * (val - 1);
            this.getequipmentData();
        },
        todoChange(val) {
            this.todoQuery.start = this.todoQuery.length * (val - 1);
            this.getTransactionNotification();
        },
        selectChange(val) {
            if (val != '设备良好') {
                this.reportCheck = true;
            } else {
                this.reportCheck = false;
            }
        },
        handleSearch() {
            this.getequipmentData();
        },
        passEdit() {
            if (this.user.password == this.user.passwordAge) {
                api.post('/login/PassEdit', { id: localStorage.getItem('id'), password: this.user.password }).then((res) => {
                    if (res == '修改成功') {
                        this.$message.success(res);
                        localStorage.clear();
                        this.$router.push('/login');
                    } else {
                        this.$message.error(res);
                    }
                });
            } else {
                this.$message.error('两次密码不同');
            }
        }
    }
};
</script>


<style scoped>
.el-row {
    margin-bottom: 20px;
}

.grid-content {
    display: flex;
    align-items: center;
    height: 100px;
}

.grid-cont-right {
    flex: 1;
    text-align: center;
    font-size: 14px;
    color: #999;
}

.grid-num {
    font-size: 30px;
    font-weight: bold;
}

.grid-con-icon {
    font-size: 50px;
    width: 100px;
    height: 100px;
    text-align: center;
    line-height: 100px;
    color: #fff;
}

.grid-con-1 .grid-con-icon {
    background: rgb(45, 140, 240);
}

.grid-con-1 .grid-num {
    color: rgb(45, 140, 240);
}

.grid-con-2 .grid-con-icon {
    background: rgb(100, 213, 114);
}

.grid-con-2 .grid-num {
    color: rgb(45, 140, 240);
}

.grid-con-3 .grid-con-icon {
    background: rgb(242, 94, 67);
}

.grid-con-3 .grid-num {
    color: rgb(242, 94, 67);
}

.user-info {
    display: flex;
    align-items: center;
    padding-bottom: 20px;
    border-bottom: 2px solid #ccc;
    margin-bottom: 10px;
}

.user-avator {
    width: 120px;
    height: 120px;
    border-radius: 50%;
}

.user-info-cont {
    padding-left: 50px;
    flex: 1;
    font-size: 14px;
    color: #999;
}

.user-info-cont div:first-child {
    font-size: 30px;
    color: #222;
}

.user-info-list {
    font-size: 14px;
    color: #999;
    line-height: 25px;
}

.user-info-list span {
    margin-left: 70px;
}

.mgb20 {
    margin-bottom: 10px;
}

.todo-item {
    font-size: 14px;
}

.todo-item-del {
    text-decoration: line-through;
    color: #999;
}

.schart {
    width: 100%;
    height: 300px;
}
.handle-input {
    width: 200px;
    margin: 0 20px;
}
</style>
