import Vue from 'vue';
import Router from 'vue-router';

Vue.use(Router);

export default new Router({
    routes: [
        {
            path: '/student',
            component: () => import(/* webpackChunkName: "home" */ '../views/UserBusiness/Dashboard.vue'),
            meta: { title: "学生系统" },
        },
        {
            path: '/Admin',
            component: () => import(/* webpackChunkName: "home" */ '../views/AdminBusiness/common/Home.vue'),
            meta: { title: '后台管理' },
            redirect: '/dashboard',
            children: [
                {
                    path: '/dashboard',
                    component: () => import(/* webpackChunkName: "dashboard" */ '../views/AdminBusiness/Dashboard.vue'),
                    meta: { title: '系统首页' },
                },
                {
                    path: '/EquipmentCategory',
                    component: () => import(/* webpackChunkName: "icon" */ '../views/AdminBusiness/EquipmentCategory.vue'),
                    meta: { title: '设备类别' }
                },
                {
                    path: '/Equipment',
                    component: () => import(/* webpackChunkName: "table" */ '../views/AdminBusiness/Equipment.vue'),
                    meta: { title: '设备信息' }
                },
                {
                    path: '/EquipmentReport',
                    component: () => import(/* webpackChunkName: "tabs" */ '../views/AdminBusiness/EquipmentReport.vue'),
                    meta: { title: '报修设备' }
                },
                {
                    path: '/EquipmentScrap',
                    component: () => import(/* webpackChunkName: "form" */ '../views/AdminBusiness/EquipmentScrap.vue'),
                    meta: { title: '报废设备' }
                },
                {
                    path: '/EquipmentUseWithUser',
                    component: () => import(/* webpackChunkName: "editor" */ '../views/AdminBusiness/EquipmentUseWithUser.vue'),
                    meta: { title: '使用申请' }
                },
                {
                    path: '/Employee',
                    component: () => import(/* webpackChunkName: "markdown" */ '../views/AdminBusiness/Employee.vue'),
                    meta: { title: '用户管理' }
                },
                {
                    path: '/TransactionNotification',
                    component: () => import(/* webpackChunkName: "upload" */ '../views/AdminBusiness/TransactionNotification.vue'),
                    meta: { title: '事务通报' }
                }
            ]
        },
        {
            path: '/login',
            component: () => import(/* webpackChunkName: "login" */ '../views/LoginBusiness/Login.vue'),
            meta: { title: '登录' }
        }
        // ,
        // {
        //     path: '*',
        //     redirect: '../views/LoginBusiness/Login.vue'
        // }
    ]
});
