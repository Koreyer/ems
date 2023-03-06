<template>
    <div class="login-wrap">
        <div class="ms-login">
            <div class="ms-title">教学设备管理系统</div>
            <el-form :model="param" :rules="rules" ref="login" label-width="0px" class="ms-content">
                <el-form-item prop="account">
                    <el-input v-model="param.account" placeholder="account">
                        <el-button slot="prepend" icon="el-icon-lx-people"></el-button>
                    </el-input>
                </el-form-item>
                <el-form-item prop="password">
                    <el-input type="password" placeholder="password" v-model="param.password" @keyup.enter.native="submitForm()">
                        <el-button slot="prepend" icon="el-icon-lx-lock"></el-button>
                    </el-input>
                </el-form-item>
                <div class="login-btn">
                    <el-button type="primary" @click="submitForm()">登录</el-button>
                </div>
                <p class="login-tips">管理员： admin 123456</p>
                <p class="login-tips">学生： 123456 123456</p>
            </el-form>
        </div>
    </div>
</template>

<script>
import api from '../../utils/request';
export default {
    data: function () {
        return {
            param: {
                account: 'admin',
                password: '123456'
            },
            rules: {
                account: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
                password: [{ required: true, message: '请输入密码', trigger: 'blur' }]
            }
        };
    },
    methods: {
        submitForm() {
            let aa = api.post('/login/login', this.param);
            aa.then((x) => {
                if (x.issuer != '登录失败') {
                    if (x.payload.role == '管理员') {
                        this.$message.success('登录成功');
                        localStorage.setItem('ms_username', x.payload.name);
                        localStorage.setItem('token', x.rawData);
                        localStorage.setItem('acc_id', x.payload.accid);
                        this.$router.push('/admin');
                    } else if (x.payload.role == '学生') {
                        this.$message.success('登录成功');
                        localStorage.setItem('ms_username', x.payload.name);
                        localStorage.setItem('token', x.rawData);
                        localStorage.setItem('acc_id', x.payload.accid);
                        localStorage.setItem('id', x.payload.id);
                        this.$router.push('/student');
                    }
                } else {
                    this.$message.error('账号或密码错误');
                }
            });
            // this.$refs.login.validate((valid) => {
            //     if (valid) {
            //         this.$message.success('登录成功');
            //         localStorage.setItem('ms_username', this.param.username);
            //         this.$router.push('/admin');
            //     } else {
            //         this.$message.error('请输入账号和密码');
            //         console.log('error submit!!');
            //         return false;
            //     }
            // });
        }
    }
};
</script>

<style scoped>
.login-wrap {
    position: relative;
    width: 100%;
    height: 100%;
    background-image: url(~/src/assets/img/login-bg.jpg);
    background-size: 100%;
}
.ms-title {
    width: 100%;
    line-height: 50px;
    text-align: center;
    font-size: 20px;
    color: #fff;
    border-bottom: 1px solid #ddd;
}
.ms-login {
    position: absolute;
    left: 50%;
    top: 50%;
    width: 350px;
    margin: -190px 0 0 -175px;
    border-radius: 5px;
    background: rgba(255, 255, 255, 0.3);
    overflow: hidden;
}
.ms-content {
    padding: 30px 30px;
}
.login-btn {
    text-align: center;
}
.login-btn button {
    width: 100%;
    height: 36px;
    margin-bottom: 10px;
}
.login-tips {
    font-size: 12px;
    line-height: 30px;
    color: #fff;
}
</style>
