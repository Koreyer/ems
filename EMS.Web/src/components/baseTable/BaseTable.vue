<template>
    <div>
        <el-table :data="tableConfig.tableData" border class="table">
            <el-table-column v-if="tableConfig.checkBox" type="selection" width="55" align="center"></el-table-column>
            <template v-for="(item, key) in tableConfig.thead">
                <el-table-column v-if="item.type == 'bool'" :key="key" :prop="item.prop" :label="item.lable" width="auto" align="center">
                    <template slot-scope="scoped">
                        <span v-if="scoped">是</span>
                        <span v-else>否</span>
                    </template>
                </el-table-column>
                <el-table-column
                    v-else-if="item.type == 'operation'"
                    :key="key"
                    :prop="item.prop"
                    :label="item.lable"
                    width="auto"
                    align="center"
                >
                    <template #default="scope">
                        <el-button size="mini" @click="edit(scope.row)" type="primary">详情</el-button>
                        <el-button size="mini" @click="onDelete(scope.row)" type="danger">删除</el-button>
                    </template>
                </el-table-column>
                <el-table-column v-else :key="key" :prop="item.prop" :label="item.lable" width="auto" align="center"> </el-table-column>
            </template>
        </el-table>
    </div>
</template>

<script>
import api from '../../utils/request';
export default {
    name: 'BaseTable',
    data() {
        return {
            tableConfig: {
                //表头
                thead: [],
                checkBox: true,
                //数据
                tableData: []
            },
            query: {
                address: '',
                name: '',
                pageIndex: 1,
                pageSize: 10
            }
        };
    },
    created() {
        let aa = api.post('/Login/GetBySearch', {
            sort: true,
            start: (this.query.pageIndex - 1) * this.query.pageSize,
            length: this.query.pageSize,
            searchValue: this.query.name
        });
        aa.then((x) => {
            this.tableConfig.tableData = x;
            console.log(this);
            console.log(this.tableConfig.tableData);
        });
    },
    methods: {
        //初始化方法
        initConfit() {
            //存在表头数据就替换
            for (let key in (this, this.config)) {
                if (Object.keys(this.tableConfig).includes(key)) {
                    this.tableConfig[key] = this.config[key];
                }
            }
        },
        //编辑
        edit(item) {
            console.log(item);
        }
        //删除
    },
    props: {
        config: {
            type: Object,
            default: () => {}
        }
    },
    watch: {
        config: {
            handler() {
                //传入数据就初始化
                this.initConfit();
            },
            immediate: true
        }
    }
};
</script> 
<style lang="scss" scoped>
</style>
