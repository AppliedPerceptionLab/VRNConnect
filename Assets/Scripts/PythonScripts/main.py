# This is a sample Python script.

# Press ⌃R to execute it or replace it with your code.
# Press Double ⇧ to search everywhere for classes, files, tool windows, actions, and settings.
import pandas as pd
import numpy as np
import bct

# importing the data
df_region_color = pd.read_csv('HCP-MMP1_RegionColor.csv', skipfooter=19, engine='python')
df_region_list = pd.read_csv('HCP-MMP1_UniqueRegionList.csv')
df_main = pd.read_csv('hcpmmp1.csv',skipfooter=19, engine='python')
ndarray_main = list(df_main).pop(0)
# columns = range(0,360)
# df_main = pd.DataFrame(data=df_main, index=columns).T
print(df_main)
# print(df_main.dtypes)
print(df_region_list)
print(df_region_color)
print(ndarray_main)

# clean_main_df = pd.DataFrame()
#
# for row in df_main.iterrows():
#      print(row.__getitem__(1).values.__getitem__(0).split(' '))

# print(clean_df)

print('Result of the algorithms:')
# print('Result of the algorithms:'+ bct.assortativity_wei(CIJ=df_main))


### addind the indices for better understanding
# result_index = df_region_list.columns

# print(result_index)

#result = pd.DataFrame(data=result, index=result_index).T
#result.to_csv('res.csv')

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
