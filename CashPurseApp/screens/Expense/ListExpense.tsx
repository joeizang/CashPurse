import React from 'react'
import { FlashList } from '@shopify/flash-list'
import { Dimensions, View, Text, Image, Pressable } from 'react-native'

const { width } = Dimensions.get('window')

const expenseList = [
  {
    id: 1,
    name: 'Expense 1',
    image: 'https://picsum.photos/200/300',
    price: 100
  },
  {
    id: 2,
    name: 'Expense 2',
    image: 'https://picsum.photos/200/300',
    price: 200
  },
  {
    id: 3,
    name: 'Expense 3',
    image: 'https://picsum.photos/200/300',
    price: 300
  },
  {
    id: 4,
    name: 'Expense 4',
    image: 'https://picsum.photos/200/300',
    price: 400
  },
  {
    id: 5,
    name: 'Expense 5',
    image: 'https://picsum.photos/200/300',
    price: 500
  },
  {
    id: 6,
    name: 'Expense 6',
    image: 'https://picsum.photos/200/300',
    price: 600
  },
  {
    id: 7,
    name: 'Expense 7',
    image: 'https://picsum.photos/200/300',
    price: 700
  },
  {
    id: 8,
    name: 'Expense 8',
    image: 'https://picsum.photos/200/300',
    price: 800
  },
  {
    id: 9,
    name: 'Expense 9',
    image: 'https://picsum.photos/200/300',
    price: 900
  },
  {
    id: 10,
    name: 'Expense 10',
    image: 'https://picsum.photos/200/300',
    price: 1000
  },
  {
    id: 11,
    name: 'Expense 11',
    image: 'https://picsum.photos/200/300',
    price: 1100
  },
  {
    id: 12,
    name: 'Expense 12',
    image: 'https://picsum.photos/200/300',
    price: 1200
  },
  {
    id: 13,
    name: 'Expense 13',
    image: 'https://picsum.photos/200/300',
    price: 1300
  },
  {
    id: 14,
    name: 'Expense 14',
    image: 'https://picsum.photos/200/300',
    price: 1400
  },
  {
    id: 15,
    name: 'Expense 15',
    image: 'https://picsum.photos/200/300',
    price: 1500
  },
  {
    id: 16,
    name: 'Expense 16',
    image: 'https://picsum.photos/200/300',
    price: 1600
  },
  {
    id: 17,
    name: 'Expense 17',
    image: 'https://picsum.photos/200/300',
    price: 1700
  },
  {
    id: 18,
    name: 'Expense 18',
    image: 'https://picsum.photos/200/300',
    price: 1800
  },
  {
    id: 19,
    name: 'Expense 19',
    image: 'https://picsum.photos/200/300',
    price: 1900
  },
  {
    id: 20,
    name: 'Expense 20',
    image: 'https://picsum.photos/200/300',
    price: 2000
  },
  {
    id: 21,
    name: 'Expense 21',
    image: 'https://picsum.photos/200/300',
    price: 2100
  },
  {
    id: 22,
    name: 'Expense 22',
    image: 'https://picsum.photos/200/300',
    price: 2200
  },
  {
    id: 23,
    name: 'Expense 23',
    image: 'https://picsum.photos/200/300',
    price: 2300
  }
]

export default function ListExpenses() {
  return (
    // <Text style={{ color: '#fff', fontSize: 35, textAlign: 'center', fontWeight: '700' }}>List Expenses</Text>
    <View
      style={{
        gap: 30,
        borderTopRightRadius: 30,
        borderTopLeftRadius: 30,
        backgroundColor: '#fff',
        marginTop: 70,
        width: width,
        height: '100%'
      }}
    >
      <View style={{ margin: 15, flex: 1 }}>
        <FlashList
          data={expenseList}
          estimatedItemSize={50}
          renderItem={({ item }) => (
            <View>
              <Pressable
                onPress={() => {
                  console.log(`Pressed ${item.name}`)
                }}
                android_ripple={{ color: '#ccc' }}
                style={{
                  flex: 1,
                  flexDirection: 'row',
                  borderColor: '#ccc',
                  marginVertical: 10,
                  justifyContent: 'space-between',
                  alignItems: 'center',
                  borderWidth: 1,
                  borderRadius: 10,
                  padding: 30,
                  backgroundColor: '#fff',
                  shadowColor: '#ccc',
                  shadowOffset: { width: 0, height: 2 },
                  shadowOpacity: 0.25,
                  shadowRadius: 3.84,
                  elevation: 5,
                  marginHorizontal: 10
                }}
              >
                <Image source={{ uri: item.image }} style={{ width: 50, height: 50, borderRadius: 50 }} />
                <Text style={{ fontSize: 15, color: '#000', fontWeight: '600' }}>{item.name}</Text>
                <Text style={{ fontSize: 15, color: '#000', fontWeight: '600' }}>{item.price}</Text>
              </Pressable>
            </View>
          )}
        />
      </View>
    </View>
  )
}
