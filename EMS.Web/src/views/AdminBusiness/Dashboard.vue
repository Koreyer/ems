<template>
    <div>
        <el-row :gutter="20">
            <el-col :span="8">
                <el-card shadow="hover" class="mgb20" style="height: 252px">
                    <div class="user-info">
                        <img src="../../assets/img/img.jpg" class="user-avator" alt />
                        <div class="user-info-cont">
                            <div class="user-info-name">{{ name }}</div>
                            <div>超级管理员</div>
                        </div>
                    </div>
                    <!-- <div class="user-info-list">
                        上次登录时间：
                        <span>2019-11-01</span>
                    </div>
                    <div class="user-info-list">
                        上次登录地点：
                        <span>东莞</span>
                    </div> -->
                </el-card>
                <el-card shadow="hover" >
                    <div slot="header" class="clearfix">
                        <span>设备使用占比</span>
                    </div>
                    <!-- 办公自动化设备 <el-progress :percentage="63.5" color="#42b983"></el-progress><br />
                    多媒体设备 <el-progress :percentage="24.1" color="#f1e05a"></el-progress><br />
                    其他设备<el-progress :percentage="12.4"></el-progress> -->
                    <div v-for="(item,index) in rate.data" :key="index">
                        {{item.name}} <el-progress :percentage="Number(((item.count/rate.count)*100).toFixed(2))" color="#42b983"></el-progress>
                    </div>
                </el-card>
            </el-col>
            <el-col :span="16">
                <el-row :gutter="20" class="mgb20">
                    <el-col :span="8">
                        <el-card shadow="hover" :body-style="{ padding: '0px' }">
                            <div class="grid-content grid-con-1">
                                <i class="el-icon-lx-people grid-con-icon"></i>
                                <div class="grid-cont-right">
                                    <div class="grid-num">{{ employeeCount }}</div>
                                    <div>用户数量</div>
                                </div>
                            </div>
                        </el-card>
                    </el-col>
                    <el-col :span="8">
                        <el-card shadow="hover" :body-style="{ padding: '0px' }">
                            <div class="grid-content grid-con-2">
                                <i class="el-icon-lx-notice grid-con-icon"></i>
                                <div class="grid-cont-right">
                                    <div class="grid-num">{{ todoQuery.total }}</div>
                                    <div>事务消息</div>
                                </div>
                            </div>
                        </el-card>
                    </el-col>
                    <el-col :span="8">
                        <el-card shadow="hover" :body-style="{ padding: '0px' }">
                            <div class="grid-content grid-con-3">
                                <i class="el-icon-lx-goods grid-con-icon"></i>
                                <div class="grid-cont-right">
                                    <div class="grid-num">{{ equipmentCount }}</div>
                                    <div>设备数量</div>
                                </div>
                            </div>
                        </el-card>
                    </el-col>
                </el-row>
                <el-card shadow="hover" style="height: 403px">
                    <el-card shadow="hover" style="height: 355px">
                        <div slot="header" class="clearfix">
                            <span>事务通报</span>
                            <el-button style="float: right; padding: 3px 0" type="text" @click="addShow = true">添加</el-button>
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
                            :page-size="todoQuery.length"
                            layout="total, prev, pager, next"
                            :total="todoQuery.total"
                        >
                        </el-pagination>
                    </el-card>
                </el-card>
            </el-col>
        </el-row>
        <!-- <el-row :gutter="20">
            <el-col :span="12">
                <el-card shadow="hover">
                    <schart ref="bar" class="schart" canvasId="bar" :options="options"></schart>
                </el-card>
            </el-col>
            <el-col :span="12">
                <el-card shadow="hover">
                    <schart ref="line" class="schart" canvasId="line" :options="options2"></schart>
                </el-card>
            </el-col>
        </el-row> -->
        <el-dialog title="添加事务通报" :visible.sync="addShow" width="30%">
            <el-form ref="form" :model="form" label-width="70px">
                <el-form-item label="标题">
                    <el-input v-model="form.name"></el-input>
                </el-form-item>
                <el-form-item label="内容">
                    <el-input v-model="form.content" type="textarea"></el-input>
                </el-form-item>
            </el-form>
            <span slot="footer" class="dialog-footer">
                <el-button @click="addShow = false">取 消</el-button>
                <el-button type="primary" @click="saveAdd">确 定</el-button>
            </span>
        </el-dialog>
    </div>
</template>

<script>
import Schart from 'vue-schart';
import bus from './common/bus';
import api from '../../utils/request';
export default {
    name: 'dashboard',
    data() {
        return {
            name: localStorage.getItem('ms_username'),
            todoList: [],
            todoQuery: {
                start: 0,
                length: 5,
                total: 0
            },
            todoCurrentPage: 1,
            addShow: false,
            equipmentCount: 0,
            employeeCount: 0,
            form: {},
            data: [
                {
                    name: '2018/09/04',
                    value: 1083
                },
                {
                    name: '2018/09/05',
                    value: 941
                },
                {
                    name: '2018/09/06',
                    value: 1139
                },
                {
                    name: '2018/09/07',
                    value: 816
                },
                {
                    name: '2018/09/08',
                    value: 327
                },
                {
                    name: '2018/09/09',
                    value: 228
                },
                {
                    name: '2018/09/10',
                    value: 1065
                }
            ],
            options: {
                type: 'bar',
                title: {
                    text: '最近一周设备使用量'
                },
                xRorate: 25,
                labels: ['周一', '周二', '周三', '周四', '周五'],
                datasets: [
                    {
                        label: '办公自动化设备',
                        data: [234, 278, 270, 190, 230]
                    },
                    {
                        label: '多媒体设备',
                        data: [164, 178, 190, 135, 160]
                    },
                    {
                        label: '其他设备',
                        data: [144, 198, 150, 235, 120]
                    }
                ]
            },
            options2: {
                type: 'line',
                title: {
                    text: '最近几个月设备使用量'
                },
                labels: ['6月', '7月', '8月', '9月', '10月'],
                datasets: [
                    {
                        label: '办公自动化设备',
                        data: [234, 278, 270, 190, 230]
                    },
                    {
                        label: '多媒体设备',
                        data: [164, 178, 150, 135, 160]
                    },
                    {
                        label: '其他设备',
                        data: [74, 118, 200, 235, 90]
                    }
                ]
            },
            rate:{
                datas:[],
                count:0
            }
        };
    },
    components: {
        Schart
    },
    computed: {},
    created() {
        // this.handleListener();
        // this.changeDate();
        this.getData();
    },
    // activated() {
    //     this.handleListener();
    // },
    // deactivated() {
    //     window.removeEventListener('resize', this.renderChart);
    //     bus.$off('collapse', this.handleBus);
    // },
    methods: {
        //获取事务通报数量
        getTransactionNotificationCount() {
            api.get('/TransactionNotification/GetBoCount').then((res) => {
                this.todoQuery.total = res;
                console.log(res);
            });
        },
        //获取事务通报
        getTransactionNotification() {
            api.get('/TransactionNotification/Get?start=' + this.todoQuery.start + '&length=' + this.todoQuery.length).then((res) => {
                this.todoList = res;
                console.log(this.todoList);
            });
        },
        //使用率
        getRate(){
            api.get('/EquipmentUseWithUser/GetRate').then(res=>{
                this.rate = res
                console.log(this.rate)
            })
        },
        getData() {
            this.getTransactionNotification();
            this.getTransactionNotificationCount();
            this.getEquipmentCount();
            this.getemployeeCount();
            this.getRate();
            console.log(1);
        },
        changeDate() {
            const now = new Date().getTime();
            this.data.forEach((item, index) => {
                const date = new Date(now - (6 - index) * 86400000);
                item.name = `${date.getFullYear()}/${date.getMonth() + 1}/${date.getDate()}`;
            });
        },
        todo(row) {
            this.todoShow = true;
            this.trans = row;
        },
        todoChange(val) {
            this.todoQuery.start = this.todoQuery.length * (val - 1);
            this.getTransactionNotification();
        },
        saveAdd() {
            this.addShow = false;
            this.form.userId = localStorage.getItem('acc_id');
            api.post('/TransactionNotification/update', this.form)
                .then((res) => {
                    this.$message.success(res.message);
                    this.getData();
                })
                .catch((err) => {
                    this.$message.error(rese.errMessage);
                });
        },
        getEquipmentCount() {
            api.get('/Equipment/GetBoCount').then((res) => {
                this.equipmentCount = res;
            });
        },
        getemployeeCount() {
            api.get('/Login/GetBoCount').then((res) => {
                this.employeeCount = res;
            });
        }
        // handleListener() {
        //     bus.$on('collapse', this.handleBus);
        //     // 调用renderChart方法对图表进行重新渲染
        //     window.addEventListener('resize', this.renderChart);
        // },
        // handleBus(msg) {
        //     setTimeout(() => {
        //         this.renderChart();
        //     }, 200);
        // },
        // renderChart() {
        //     this.$refs.bar.renderChart();
        //     this.$refs.line.renderChart();
        // }
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
    margin-bottom: 20px;
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
    margin-bottom: 20px;
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
</style>
